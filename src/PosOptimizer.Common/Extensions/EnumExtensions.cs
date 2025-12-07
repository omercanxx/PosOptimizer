using System.ComponentModel;
using System.Reflection;

namespace PosOptimizer.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        if (name == null)
            return value.ToString();

        var field = type.GetField(name);

        if (field == null)
            return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }
}