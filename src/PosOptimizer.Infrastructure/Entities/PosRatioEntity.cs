using PosOptimizer.Common.Enums;

namespace PosOptimizer.Infrastructure.Entities;

public class PosRatioEntity : BaseEntity
{   
    public PosName PosName { get; set; }

    public CardType CardType { get; set; }

    public CardBrand CardBrand { get; set; }

    public Currency Currency { get; set; }

    public int Installment { get; set; }

    public decimal CommissionRate { get; set; }

    public decimal MinFee { get; set; }

    public int Priority { get; set; }
}