namespace Ng.Pass.Server.Core.Interfaces;

public interface IGeographicCoordinateRequest
{
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
