using EventManager.Application.Requests.Location;
using EventManager.Application.Responses.Location;

namespace EventManager.Application.Contracts;

public interface ILocationService
{
    public Task<IList<LocationResponse>> GetLocations(CancellationToken cancellationToken);
    public Task<LocationResponse> CreateLocation(CreateLocationRequest locationRequest, CancellationToken cancellationToken);
    public Task<LocationResponse> UpdateLocation(UpdateLocationRequest locationRequest, int locationId, CancellationToken cancellationToken);
    public Task<bool> DeleteLocation(int locationId, CancellationToken cancellationToken);
}
