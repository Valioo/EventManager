using EventManager.Domain.Entities;
using EventManager.Domain;
using EventManager.Application.Contracts;
using EventManager.Application.Responses.Location;
using EventManager.Application.Requests.Location;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class LocationService : ILocationService
{
    private readonly AppDbContext _appDbContext;

    public LocationService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<LocationResponse> CreateLocation(CreateLocationRequest locationRequest, CancellationToken cancellationToken)
    {
        var location = new Location(locationRequest.Address, locationRequest.City, locationRequest.VenueName);

        await _appDbContext.Locations.AddAsync(location, cancellationToken);
        await _appDbContext.SaveChangesAsync();

        return new LocationResponse(location);
    }

    public async Task<bool> DeleteLocation(int locationId, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Locations
                                .Where(x => x.Id == locationId)
                                .ExecuteDeleteAsync(cancellationToken);

        return category > 0;
    }

    public async Task<IList<LocationResponse>> GetLocations(CancellationToken cancellationToken)
    {
        return await _appDbContext.Locations
                                .Select(x => new LocationResponse(x))
                                .ToListAsync(cancellationToken);
    }

    public async Task<LocationResponse> UpdateLocation(UpdateLocationRequest locationRequest, int locationId, CancellationToken cancellationToken)
    {
        var location = await _appDbContext.Locations
                                .FirstOrDefaultAsync(x => x.Id == locationId, cancellationToken);

        if (location is null)
        {
            return null!;
        }

        var performSave = false;

        if (!string.IsNullOrWhiteSpace(locationRequest.City))
        {
            location.City = locationRequest.City;
            performSave = true;
        }

        if (!string.IsNullOrWhiteSpace(locationRequest.VenueName))
        {
            location.VenueName = locationRequest.VenueName;
            performSave = true;
        }

        if (!string.IsNullOrWhiteSpace(locationRequest.Address))
        {
            location.Address = locationRequest.Address;
            performSave = true;
        }

        if (performSave)
        {
            await _appDbContext.SaveChangesAsync();
        }

        return new LocationResponse(location);
    }
}
