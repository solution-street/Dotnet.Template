namespace Ng.Pass.Server.Core.Interfaces;

/// <summary>
/// A shared interfance used for the request, response, and CiviCRM search command.
/// </summary>
public interface IContactSearchModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? OrganizationName { get; set; }
}
