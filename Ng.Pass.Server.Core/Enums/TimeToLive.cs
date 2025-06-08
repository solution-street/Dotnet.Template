using Ng.Pass.Server.Core.Attributes;

namespace Ng.Pass.Server.Core.Enums;

public enum TimeToLive
{
    [StringValue("week")]
    Week,

    [StringValue("day")]
    Day,

    [StringValue("hour")]
    Hour
}
