using System.Reflection;
using Dotnet.Template.Server.Core.Attributes;

namespace Dotnet.Template.Server.Core.Extensions;

public static class EnumerationExtensions
{
    public static bool IsIn<T>(this T enumValue, params T[] expectedValues)
        where T : Enum => expectedValues.Contains(enumValue);

    public static string GetStringValue(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());

        var attribute = field?.GetCustomAttribute<StringValueAttribute>();

        return attribute?.Value ?? enumValue.ToString();
    }
}
