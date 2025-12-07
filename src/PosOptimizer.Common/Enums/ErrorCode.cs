using System.ComponentModel;

namespace PosOptimizer.Common.Enums;

public enum ErrorCode
{
    [Description("Bilinmeyen bir hata oluştu.")]
    UnknownError = 1000,
    
    [Description("Yetkisiz erişim.")]
    Unauthorized = 1001,

    [Description("Bu işlem için izniniz yok.")]
    Forbidden = 1002,
    
    [Description("Geçersiz veya eksik parametre.")]
    ValidationError = 1003,
    
    [Description("Uygun POS oranı bulunamadı.")]
    PosRatioNotFound = 1004,
    
    [Description("Taksit sayısı 0'dan büyük olmalıdır.")]
    InstallmentMustBeGreaterThanZero = 1005,

    [Description("Para birimi zorunludur.")]
    CurrencyIsRequired = 1006,

    [Description("Tutar 0'dan büyük olmalıdır.")]
    AmountMustBeGreaterThanZero = 1007
}