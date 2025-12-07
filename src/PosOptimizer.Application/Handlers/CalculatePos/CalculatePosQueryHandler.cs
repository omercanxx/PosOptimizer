using MediatR;
using PosOptimizer.Application.Models.Queries;
using PosOptimizer.Application.Models.Responses;
using PosOptimizer.Application.Services.Abstractions;
using PosOptimizer.Common;

namespace PosOptimizer.Application.Handlers.CalculatePos;

public class CalculatePosQueryHandler : IRequestHandler<CalculatePosQuery, ApiResult<PosRatioResponseDto>>
{
    private readonly IPosRatioService _posRatioService;
    
    public CalculatePosQueryHandler(IPosRatioService posRatioService)
    {
        _posRatioService = posRatioService;
    }
    
    public async Task<ApiResult<PosRatioResponseDto>> Handle(CalculatePosQuery query, CancellationToken cancellationToken)
    {
        return await _posRatioService.CalculatePosAsync(query);
    }
}