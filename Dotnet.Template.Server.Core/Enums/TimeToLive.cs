using Dotnet.Template.Server.Core.Attributes;

namespace Dotnet.Template.Server.Core.Enums;

public enum TimeToLive
{
    [StringValue("week")]
    Week,

    [StringValue("day")]
    Day,

    [StringValue("hour")]
    Hour
}
