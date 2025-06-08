using System.ComponentModel.DataAnnotations;

namespace Ng.Pass.Server.Core.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute : Attribute
{
    public string Value { get; }

    public StringValueAttribute(string value)
    {
        Value = value;
    }
}
