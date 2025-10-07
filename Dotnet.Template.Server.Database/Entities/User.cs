using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Dotnet.Template.Server.Database.Interfaces;

namespace Dotnet.Template.Server.Database.Entities;

[Table("User", Schema = "users")]
[Index(nameof(AuthProviderId), IsUnique = true)]
public class User : ITimestampedEntity
{
    [Key]
    public Guid Id { get; set; }

    public string AuthProviderId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [InverseProperty(nameof(Secret.User))]
    public virtual ICollection<Secret> Secrets { get; set; } = new List<Secret>();
}
