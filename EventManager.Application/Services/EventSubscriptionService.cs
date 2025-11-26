using EventManager.Application.Contracts;
using EventManager.Application.Responses.Events;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class EventSubscriptionService : IEventSubscriptionService
{
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _appDbContext;

    public EventSubscriptionService(ICurrentUserService currentUser, AppDbContext appDbContext)
    {
        _currentUser = currentUser;
        _appDbContext = appDbContext;
    }

    public async Task<IList<EventSubscriptionResponse>> GetAll(CancellationToken cancellationToken)
    {
        return await _appDbContext.EventSubscriptions
                            .Include(x => x.Event)
                            .Where(x => x.UserId == _currentUser.UserId)
                            .Select(x => new EventSubscriptionResponse(x))
                            .ToListAsync();
    }

    public async Task<bool> SubscribeToEvent(int eventId, CancellationToken cancellationToken)
    {
        var eventExists = await _appDbContext.Events.AnyAsync(x => x.Id == eventId, cancellationToken);
        var subscriptionExists = await _appDbContext.EventSubscriptions.AnyAsync(x => x.UserId == _currentUser.UserId && x.EventId == eventId, cancellationToken);

        if (!eventExists || subscriptionExists)
        {
            return false;
        }

        var eventSub = new EventSubscription(eventId, _currentUser.UserId!.Value);
        await _appDbContext.EventSubscriptions.AddAsync(eventSub, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UnsubscribeFromEvent(int eventId, CancellationToken cancellationToken)
    {
        var result = await _appDbContext.EventSubscriptions
                    .Where(x => x.EventId == eventId && x.UserId == _currentUser.UserId)
                    .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }
}