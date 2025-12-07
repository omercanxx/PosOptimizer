using System.ComponentModel;

namespace PosOptimizer.Common.Enums;

public enum Currency
{
    [Description("Türk Lirası")]
    TRY = 1,

    [Description("Amerikan Doları")]
    USD = 2,

    [Description("Euro")]
    EUR = 3
}
