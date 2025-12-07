using MediatR;
using PosOptimizer.Application.Models.Responses;
using PosOptimizer.Common;
using PosOptimizer.Common.Enums;

namespace PosOptimizer.Application.Models.Queries;

public class CalculatePosQuery : IRequest<ApiResult<PosRatioResponseDto>>
{
    public decimal Amount { get; set; }
    
    public int Installment { get; set; }
    
    public Currency Currency { get; set; }
    
    public CardType? CardType  { get; set; }
    
    public CardBrand? CardBrand  { get; set; }
}