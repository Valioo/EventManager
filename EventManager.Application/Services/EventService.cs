using EventManager.Application.Contracts;
using EventManager.Application.Requests.Events;
using EventManager.Application.Responses.Events;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _appDbContext;

    public EventService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<EventResponse> CreateEvent(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var location = await _appDbContext.Locations
                                .FirstOrDefaultAsync(x => x.Id == request.LocationId, cancellationToken);
        
        if (location is null)
        {
            return null!;
        }

        var category = await _appDbContext.Categories
                                .FirstOrDefaultAsync(x => x.Id == request.CategoryId);

        if (category is null)
        {
            return null!;
        }

        var newEvent = new Event(request.Title!, request.Description!, 
                    request.StartDate!.Value, request.EndDate!.Value, request.CategoryId!.Value, request.LocationId!.Value);

        await _appDbContext.AddAsync(newEvent, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return new EventResponse(newEvent);
    }

    public async Task<bool> DeleteEvent(int id, CancellationToken cancellationToken)
    {
        var existing = await _appDbContext.Events.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (existing is null)
        {
            return false;
        }

        existing.IsDeleted = true;
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<EventResponse> GetById(int id, CancellationToken cancellationToken)
    {
        var existing = await _appDbContext.Events
                                    .Include(x => x.Category)
                                    .Include(x => x.Location)
                                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return existing is not null ? new EventResponse(existing) : null!;
    }

    public async Task<EventResponse> UpdateEvent(UpdateEventRequest request, int id, CancellationToken cancellationToken)
    {
        var cEvent = await _appDbContext.Events
                            .Include(x => x.Category)
                            .Include(x => x.Location)
                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (cEvent is null)
        {
            return null!;
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            cEvent.Title = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            cEvent.Description = request.Description;
        }

        if (request.StartDate.HasValue)
        {
            cEvent.StartDate = request.StartDate.Value;
        }

        if (request.EndDate.HasValue)
        {
            cEvent.EndDate = request.EndDate.Value;
        }

        if (cEvent.StartDate > cEvent.EndDate)
        {
            return null!;
        }

        if (request.CategoryId.HasValue && cEvent.CategoryId != request.CategoryId.Value)
        {
            var category = await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId.Value, cancellationToken);

            if (category is null)
            {
                return null!;
            }

            cEvent.CategoryId = category.Id;
            cEvent.Category = category;
        }

        if (request.LocationId.HasValue && cEvent.LocationId != request.LocationId.Value)
        {
            var location = await _appDbContext.Locations.FirstOrDefaultAsync(x => x.Id == request.LocationId.Value, cancellationToken);

            if (location is null)
            {
                return null!;
            }

            cEvent.LocationId = location.Id;
            cEvent.Location = location;
        }

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return new EventResponse(cEvent);
    }
}