using Moq;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PosOptimizer.Application.Models.Queries;
using PosOptimizer.Application.Services;
using PosOptimizer.Application.Services.Abstractions;
using PosOptimizer.Common.Enums;
using PosOptimizer.Infrastructure.Entities;
using PosOptimizer.Infrastructure.Repositories;
using PosOptimizer.MockApiClient.Responses;
using PosOptimizer.MockApiClient;

namespace PosOptimizer.Test.Tests;

public class PosRatioServiceTests
{
    private Mock<ILogger<PosRatioService>> _logger;
    private Mock<IRedisCacheService> _redis;
    private Mock<IMockApiClient> _client;
    private Mock<IPosRatioRepository> _repo;
    private PosRatioService _service;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<PosRatioService>>();
        _redis = new Mock<IRedisCacheService>();
        _client = new Mock<IMockApiClient>();
        _repo = new Mock<IPosRatioRepository>();

        _service = new PosRatioService(
            _logger.Object,
            _redis.Object,
            _client.Object,
            _repo.Object
        );
    }

    #region ExecuteTask
    
    [Test]
    public async Task ExecuteTask_ShouldClearCacheFetchApiSetCacheAndInsertDb()
    {
        // Arrange
        var ratios = new List<PosRatioDto>
        {
            new()
            {
                PosName = PosName.Akbank,
                CardType = CardType.Credit,
                CardBrand = CardBrand.Maximum,
                Installment = 3,
                Currency = Currency.TRY,
                CommissionRate = 0.05m,
                MinFee = 2,
                Priority = 1
            }
        };

        _client.Setup(x => x.GetRatiosAsync())
            .ReturnsAsync(ratios);

        // Act
        await _service.ExecuteTask();

        // Assert
        _redis.Verify(x => x.RemoveAsync("PosRatios"), Times.Once);
        _redis.Verify(x => x.SetAsync("PosRatios", ratios, It.IsAny<TimeSpan>()), Times.Once);
        _repo.Verify(x => x.AddAsync(It.IsAny<List<PosRatioEntity>>()), Times.Once);
    }

    #endregion
    
    #region CalculatePosAsync Tests

    [Test]
    public async Task CalculatePosAsync_ShouldUseCachedValue_WhenCacheExists()
    {
        // Arrange
        var ratios = new List<PosRatioDto>
        {
            new()
            {
                PosName = PosName.Akbank,
                Installment = 3,
                Currency = Currency.TRY,
                CommissionRate = 0.1m,
                MinFee = 5,
                Priority = 1
            }
        };

        _redis.Setup(x => x.GetAsync<List<PosRatioDto>>("PosRatios"))
            .ReturnsAsync(ratios);

        var query = new CalculatePosQuery
        {
            Amount = 100,
            Installment = 3,
            Currency = Currency.TRY
        };

        // Act
        var result = await _service.CalculatePosAsync(query);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.PosName.Should().Be(PosName.Akbank);
        result.Data.Commission.Should().Be(10m);
    }

    [Test]
    public async Task CalculatePosAsync_ShouldFetchFromApiOnCacheMiss()
    {
        // Arrange
        _redis.Setup(x => x.GetAsync<List<PosRatioDto>>("PosRatios"))
            .ReturnsAsync((List<PosRatioDto>?)null);

        var apiRatios = new List<PosRatioDto>
        {
            new()
            {
                PosName = PosName.Denizbank,
                Installment = 2,
                Currency = Currency.TRY,
                CommissionRate = 0.05m,
                MinFee = 1,
                Priority = 5
            }
        };

        _client.Setup(x => x.GetRatiosAsync()).ReturnsAsync(apiRatios);

        var query = new CalculatePosQuery
        {
            Amount = 200,
            Installment = 2,
            Currency = Currency.TRY
        };

        // Act
        var result = await _service.CalculatePosAsync(query);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        _redis.Verify(x => x.SetAsync("PosRatios", apiRatios, It.IsAny<TimeSpan>()), Times.Once);
        result.Data.PosName.Should().Be(PosName.Denizbank);
    }

    [Test]
    public async Task CalculatePosAsync_ShouldReturnFail_WhenNoMatchingRatio()
    {
        // Arrange
        _redis.Setup(x => x.GetAsync<List<PosRatioDto>>("PosRatios"))
            .ReturnsAsync(new List<PosRatioDto>());

        var query = new CalculatePosQuery
        {
            Amount = 100,
            Installment = 3,
            Currency = Currency.TRY
        };

        // Act
        var result = await _service.CalculatePosAsync(query);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorCode.Should().Be(ErrorCode.PosRatioNotFound);
        result.Data.Should().BeNull();
    }

    [Test]
    public async Task CalculatePosAsync_ShouldPickLowestCost_WhenMultipleMatches()
    {
        // Arrange
        var ratios = new List<PosRatioDto>
        {
            new()
            {
                PosName = PosName.Akbank,
                Installment = 3,
                Currency = Currency.TRY,
                CommissionRate = 0.05m,
                MinFee = 2,
                Priority = 1
            },
            new()
            {
                PosName = PosName.Denizbank,
                Installment = 3,
                Currency = Currency.TRY,
                CommissionRate = 0.20m,
                MinFee = 10,
                Priority = 10
            }
        };

        _redis.Setup(x => x.GetAsync<List<PosRatioDto>>("PosRatios")).ReturnsAsync(ratios);

        var query = new CalculatePosQuery
        {
            Amount = 100,
            Installment = 3,
            Currency = Currency.TRY
        };

        // Act
        var result = await _service.CalculatePosAsync(query);
        
        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.PosName.Should().Be(PosName.Akbank);
    }

    [Test]
    public async Task CalculatePosAsync_ShouldApplyCurrencyMultiplier_WhenCurrencyNotTRY()
    {
        // Arrange
        var ratios = new List<PosRatioDto>
        {
            new()
            {
                PosName = PosName.Akbank,
                Installment = 1,
                Currency = Currency.USD,
                CommissionRate = 0.10m,
                MinFee = 1,
                Priority = 1
            }
        };

        _redis.Setup(x => x.GetAsync<List<PosRatioDto>>("PosRatios")).ReturnsAsync(ratios);

        var query = new CalculatePosQuery
        {
            Amount = 100,
            Installment = 1,
            Currency = Currency.USD
        };

        // Act
        var result = await _service.CalculatePosAsync(query);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Commission.Should().Be(10.1m);
    }
    
    #endregion
}