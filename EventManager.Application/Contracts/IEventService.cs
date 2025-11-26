using EventManager.Application.Helpers.Pagination;
using EventManager.Application.Requests.Events;
using EventManager.Application.Responses.Events;

namespace EventManager.Application.Contracts;

public interface IEventService
{
    public Task<PaginatedResponse<EventResponse>> Search(EventSearchRequest request, PaginationQuery pagination, CancellationToken cancellationToken);
    public Task<EventResponse> GetById(int id, CancellationToken cancellationToken);
    public Task<EventResponse> CreateEvent(CreateEventRequest request, CancellationToken cancellationToken);
    public Task<EventResponse> UpdateEvent(UpdateEventRequest request, int id, CancellationToken cancellationToken);
    public Task<bool> DeleteEvent(int id, CancellationToken cancellationToken);
    public Task<bool> AttachTag(int eventId, int tagId, CancellationToken cancellationToken);
    public Task<bool> DeleteTagFromEvent(int eventId, int tagId, CancellationToken cancellationToken);
}