using System.Text.Json.Serialization;
using Dotnet.Template.Server.Core.Attributes;

namespace Dotnet.Template.Server.Core.Models;

public class BaseAuthenticatedRequest
{
    [JsonIgnore]
    [IgnoreValidation]
    public Executor Executor { get; set; } = null!;
}
