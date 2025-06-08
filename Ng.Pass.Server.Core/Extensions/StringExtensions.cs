using System.Net;

namespace Ng.Pass.Server.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Trims the specified field and encodes it for use in a query string.
    /// Encapsulation is formatted for a CONTAINS clause by wrapping the trimmed value in percent symbols ("%").
    /// </summary>
    /// <param name="field">The field name to be trimmed and encoded. Can be null.</param>
    /// <returns>The trimmed and URL-encoded string, or an encoded wildcard ("%") if the input is null or empty.</returns>
    public static string TrimAndEncodeForQuery(this string? field) => WebUtility.UrlEncode($"%{field?.Trim() ?? ""}%");

    /// <summary>
    /// Parses string representations of a ULS-ONLY date and time to a DateTime object (and sets the DateTimeKind to UTC).
    /// This is useful when an external system's date time format is known to be UTC - then we need to set the DateTimeKind to ensure
    /// that the database understands the date time as UTC while saving.
    /// <para>
    /// Note: this method should only be used against ULS-formatted dates. We are confident of the ULS's date time format, but in other systems
    /// the format may vary.
    /// </para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime? TryParseUlsUtcDateTime(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (DateTime.TryParse(value, out DateTime result))
        {
            return DateTime.SpecifyKind(result, DateTimeKind.Utc);
        }

        return null;
    }
}
