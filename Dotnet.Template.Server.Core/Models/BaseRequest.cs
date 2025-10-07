using System.Text.Json.Serialization;
using Dotnet.Template.Server.Core.Attributes;

namespace Dotnet.Template.Server.Core.Models;

public class BaseRequest
{
    [JsonIgnore]
    [IgnoreValidation]
    public Executor? Executor { get; set; }
}
