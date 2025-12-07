using Microsoft.Extensions.Logging;
using PosOptimizer.Application.Models;
using PosOptimizer.Application.Models.Queries;
using PosOptimizer.Application.Models.Responses;
using PosOptimizer.Application.Services.Abstractions;
using PosOptimizer.Common;
using PosOptimizer.Common.Enums;
using PosOptimizer.Infrastructure.Entities;
using PosOptimizer.Infrastructure.Repositories;
using PosOptimizer.MockApiClient;
using PosOptimizer.MockApiClient.Responses;

namespace PosOptimizer.Application.Services;

public class PosRatioService : IPosRatioService
{
    private const string RedisKey = "PosRatios";
    private readonly TimeSpan _expiration = TimeSpan.FromDays(1);

    private readonly ILogger<PosRatioService> _logger;
    private readonly IRedisCacheService _redisCacheService;
    private readonly IMockApiClient _client;
    private readonly IPosRatioRepository  _posRatioRepository;

    public PosRatioService(
        ILogger<PosRatioService> logger,
        IRedisCacheService redisCacheService,
        IMockApiClient client,
        IPosRatioRepository posRatioRepository)
    {
        _logger = logger;
        _redisCacheService = redisCacheService;
        _client = client;
        _posRatioRepository = posRatioRepository;
    }

    public async Task ExecuteTask()
    {
        await _redisCacheService.RemoveAsync(RedisKey);

        _logger.LogInformation("Cleared existing POS ratio cache.");

        var ratios = await _client.GetRatiosAsync();

        _logger.LogInformation("Fetched ratio records");

        await _redisCacheService.SetAsync(RedisKey, ratios, _expiration);
        
        _logger.LogInformation("Set POS ratio cache successfully.");
        
        var ratioEntities = ratios.Select(x => new PosRatioEntity()
        {
            PosName = x.PosName,
            CardType = x.CardType,
            CardBrand = x.CardBrand,
            Installment = x.Installment,
            Currency = x.Currency,
            CommissionRate = x.CommissionRate,
            MinFee = x.MinFee,
            Priority = x.Priority
        }).ToList();

        await _posRatioRepository.AddAsync(ratioEntities);

        _logger.LogInformation("Inserted POS ratio records to database.");
    }
    
    public async Task<ApiResult<PosRatioResponseDto>> CalculatePosAsync(CalculatePosQuery request)
    {
        var ratios = await _redisCacheService.GetAsync<List<PosRatioDto>>(RedisKey);

        if (ratios is null)
        {
            _logger.LogInformation("Cache miss on POS ratios. Fetching from provider.");
        
            ratios = await _client.GetRatiosAsync();

            if (!ratios.Any())
                return ApiResult<PosRatioResponseDto>.Fail(ErrorCode.PosRatioNotFound);
            
            await _redisCacheService.SetAsync(RedisKey, ratios, _expiration);
        }

        var matchings = ratios
            .Where(x =>
                x.Installment == request.Installment &&
                x.Currency == request.Currency &&
                (request.CardType == null || x.CardType == request.CardType) &&
                (request.CardBrand == null || x.CardBrand == request.CardBrand))
            .Select(x => new PosRatioMatchDto()
            {
                PosName = x.PosName,
                Currency = x.Currency,
                CommissionRate = x.CommissionRate,
                MinFee = x.MinFee,
                Priority = x.Priority,
                Commission = CalculateCommission(request.Amount, x.CommissionRate, x.Currency),
                Cost = CalculateCost(request.Amount, x.CommissionRate, x.MinFee, x.Currency)
            })
            .ToList();

        if (!matchings.Any())
            return ApiResult<PosRatioResponseDto>.Fail(ErrorCode.PosRatioNotFound);
        
        var selected = matchings
            .OrderBy(c => c.Cost)
            .ThenByDescending(c => c.Priority)
            .ThenBy(c => c.CommissionRate)
            .ThenBy(c => c.PosName)
            .First();

        return ApiResult<PosRatioResponseDto>.Ok(new PosRatioResponseDto
        {
            PosName = selected.PosName,
            Cost = selected.Cost,
            Commission = selected.Commission
        });
    }

    #region Private Methods

    private decimal CalculateCommission(decimal amount, decimal commissionRate, Currency currency)
    {
        var commission = amount * commissionRate;

        if (currency != Currency.TRY)
            commission *= 1.01m;

        return decimal.Round(commission, 2);
    }
    
    private decimal CalculateCost(decimal amount, decimal commissionRate, decimal minFee, Currency currency)
    {
        var commission = CalculateCommission(amount, commissionRate, currency);

        var cost = Math.Max(commission, minFee);

        return decimal.Round(cost, 2);
    }

    #endregion
}