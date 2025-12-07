using PosOptimizer.Common.Enums;

namespace PosOptimizer.Application.Models;

public class PosRatioMatchDto
{
    public PosName PosName { get; set; }

    public Currency Currency { get; set; }

    public decimal CommissionRate { get; set; }

    public decimal MinFee { get; set; }

    public int Priority { get; set; }
    
    public decimal Cost { get; set; }
    
    public decimal Commission { get; set; }
}