using System.Runtime.Serialization;

namespace PosOptimizer.Common.Enums;

public enum PosName
{
    [EnumMember(Value = "Garanti")]
    Garanti = 1,

    YapiKredi = 2,

    Akbank = 3,

    Isbank = 4,

    Vakifbank = 5,

    Ziraat = 6,

    Halkbank = 7,

    QNB = 8,

    Denizbank = 9,

    KuveytTurk = 10
}