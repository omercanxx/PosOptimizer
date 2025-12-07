using PosOptimizer.Application.Models.Queries;
using PosOptimizer.Application.Models.Responses;
using PosOptimizer.Common;

namespace PosOptimizer.Application.Services.Abstractions;

public interface IPosRatioService
{
    Task ExecuteTask();

    Task<ApiResult<PosRatioResponseDto>> CalculatePosAsync(CalculatePosQuery request);
}