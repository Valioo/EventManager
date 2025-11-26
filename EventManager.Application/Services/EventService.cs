using EventManager.Application.Contracts;
using EventManager.Application.Helpers.Pagination;
using EventManager.Application.Requests.Events;
using EventManager.Application.Responses.Events;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _appDbContext;
    private readonly ITagService _tagService;

    public EventService(AppDbContext appDbContext, ITagService tagService)
    {
        _appDbContext = appDbContext;
        _tagService = tagService;
    }

    public async Task<bool> AttachTag(int eventId, int tagId, CancellationToken cancellationToken)
    {
        var evt = await _appDbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId, cancellationToken);
        var tag = await _appDbContext.Tags.FirstOrDefaultAsync(x => x.Id == tagId, cancellationToken);

        if (evt is null || tag is null)
        {
            return false;
        }

        if (await _appDbContext.EventTags.AnyAsync(x => x.TagId == tagId && x.EventId == eventId, cancellationToken))
        {
            return false;
        }

        var eventTag = new EventTag(eventId, tagId);

        await _appDbContext.EventTags.AddAsync(eventTag, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
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

    public async Task<bool> DeleteTagFromEvent(int eventId, int tagId, CancellationToken cancellationToken)
    {
        var result = await _appDbContext.EventTags
                                .Where(x => x.EventId == eventId && x.TagId == tagId)
                                .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
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

    public async Task<PaginatedResponse<EventResponse>> Search(
        EventSearchRequest request,
        PaginationQuery pagination,
        CancellationToken cancellationToken)
    {
        var query = _appDbContext.Events
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Location)
            .Include(e => e.EventTags)
                .ThenInclude(et => et.Tag)
            .AsQueryable();

        if (request.CategoryId is not null)
        {
            query = query.Where(e => e.CategoryId == request.CategoryId.Value);
        }

        if (request.LocationId is not null)
        {
            query = query.Where(e => e.LocationId == request.LocationId.Value);
        }

        if (request.TagIds is not null && request.TagIds.Count != 0)
        {
            var tagIds = request.TagIds;

            query = query.Where(e =>
                tagIds.All(tagId =>
                    e.EventTags.Any(et => et.TagId == tagId)
                )
            );
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var maxPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

        var events = await query
            .OrderBy(e => e.StartDate)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        var eventResponses = events
            .Select(e => new EventResponse(e))
            .ToList();

        return new PaginatedResponse<EventResponse>
        {
            Result = eventResponses,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            MaximumPages = maxPages
        };
    }
}