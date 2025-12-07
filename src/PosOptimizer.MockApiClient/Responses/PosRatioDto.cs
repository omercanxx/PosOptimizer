using System.Text.Json.Serialization;
using PosOptimizer.Common.Enums;

namespace PosOptimizer.MockApiClient.Responses;

public class PosRatioDto
{
    [JsonPropertyName("pos_name")]
    public PosName PosName { get; set; }

    [JsonPropertyName("card_type")]
    public CardType CardType { get; set; }

    [JsonPropertyName("card_brand")]
    public CardBrand CardBrand { get; set; }

    [JsonPropertyName("currency")]
    public Currency Currency { get; set; }

    [JsonPropertyName("installment")]
    public int Installment { get; set; }

    [JsonPropertyName("commission_rate")]
    public decimal CommissionRate { get; set; }

    [JsonPropertyName("min_fee")]
    public decimal MinFee { get; set; }

    [JsonPropertyName("priority")]
    public int Priority { get; set; }
}