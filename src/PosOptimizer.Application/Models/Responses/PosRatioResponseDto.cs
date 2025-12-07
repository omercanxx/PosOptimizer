using PosOptimizer.Common.Enums;

namespace PosOptimizer.Application.Models.Responses;

public class PosRatioResponseDto
{
    public PosName PosName { get; set; }
    
    public decimal Cost { get; set; }

    public decimal Commission { get; set; }
}