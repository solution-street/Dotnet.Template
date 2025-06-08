namespace Ng.Pass.Server.Core.Interfaces;

public interface IUserStatus
{
    public bool? IsApproved { get; set; }
    public bool IsActive { get; set; }
}
