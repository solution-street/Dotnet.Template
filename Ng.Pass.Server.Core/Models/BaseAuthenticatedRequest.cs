using System.Text.Json.Serialization;
using Ng.Pass.Server.Core.Attributes;

namespace Ng.Pass.Server.Core.Models;

public class BaseAuthenticatedRequest
{
    [JsonIgnore]
    [IgnoreValidation]
    public Executor Executor { get; set; } = null!;
}
