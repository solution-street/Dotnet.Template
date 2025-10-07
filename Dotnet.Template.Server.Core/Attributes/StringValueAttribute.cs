using System.ComponentModel.DataAnnotations;

namespace Dotnet.Template.Server.Core.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute : Attribute
{
    public string Value { get; }

    public StringValueAttribute(string value)
    {
        Value = value;
    }
}
