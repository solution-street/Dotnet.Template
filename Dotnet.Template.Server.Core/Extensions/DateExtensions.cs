namespace Dotnet.Template.Server.Core.Extensions;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }
}
