using EventManager.Application.Responses.Events;

namespace EventManager.Application.Contracts;

public interface IEventSubscriptionService
{
    public Task<bool> SubscribeToEvent(int eventId, CancellationToken cancellationToken);
    public Task<bool> UnsubscribeFromEvent(int eventId, CancellationToken cancellationToken);
    public Task<IList<EventSubscriptionResponse>> GetAll(CancellationToken cancellationToken);
}