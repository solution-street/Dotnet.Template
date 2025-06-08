using System.Text.RegularExpressions;

namespace Ng.Pass.Server.Common.Tests.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Takes a property name (nameof({propertyName}) and adds space characters between uppercased characters. This is useful
    /// to replicate how Fluent Validation handles generating is {PropertyName} for use in a message.
    /// <para></para>
    /// Example: A property called LicenseeFirstName -> Licensee First Name
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static string GetDisplayName(this string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return propertyName;

        // Regex to insert a space before each uppercase letter except for the first character
        return Regex.Replace(propertyName, "(\\B[A-Z])", " $1");
    }
}
