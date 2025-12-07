using Microsoft.Extensions.Logging;
using PosOptimizer.Application.Services.Abstractions;

namespace PosOptimizer.Job;

public class PosRatioFetchJob
{
    private readonly ILogger<PosRatioFetchJob> _logger;
    private readonly IPosRatioService _posRatioService;

    public PosRatioFetchJob(
        ILogger<PosRatioFetchJob> logger,
        IPosRatioService posRatioService)
    {
        _logger = logger;
        _posRatioService = posRatioService;
    }

    public async Task Execute()
    {
        _logger.LogInformation("Starting POS ratio synchronization job.");

        await _posRatioService.ExecuteTask();
        
        _logger.LogInformation("POS ratio synchronization job completed.");
    }
}