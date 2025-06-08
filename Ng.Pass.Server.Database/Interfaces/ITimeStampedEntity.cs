namespace Ng.Pass.Server.Database.Interfaces;

public interface ITimestampedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
