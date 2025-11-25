using EventManager.Application.Requests.Events;
using EventManager.Application.Responses.Events;

namespace EventManager.Application.Contracts;

public interface IEventService
{
    public Task<EventResponse> GetById(int id, CancellationToken cancellationToken);
    public Task<EventResponse> CreateEvent(CreateEventRequest request, CancellationToken cancellationToken);
    public Task<EventResponse> UpdateEvent(UpdateEventRequest request, int id, CancellationToken cancellationToken);
    public Task<bool> DeleteEvent(int id, CancellationToken cancellationToken);
}