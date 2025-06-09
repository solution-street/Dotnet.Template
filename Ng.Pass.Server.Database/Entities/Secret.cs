using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ng.Pass.Server.Database.Interfaces;

namespace Ng.Pass.Server.Database.Entities;

[Table("Secret", Schema = "secret")]
public class Secret : ITimestampedEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("User")]
    public Guid? UserId { get; set; }

    public string Value { get; set; } = null!;

    public string Ttl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [InverseProperty(nameof(User.Secrets))]
    public virtual User? User { get; set; }
}
