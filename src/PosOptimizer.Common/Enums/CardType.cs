using System.ComponentModel;

namespace PosOptimizer.Common.Enums;

public enum CardType
{
    [Description("Kredi Kartı")]
    Credit = 1,

    [Description("Banka Kartı (Debit)")]
    Debit = 2
}
