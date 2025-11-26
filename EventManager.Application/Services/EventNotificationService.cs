using EventManager.Application.Contracts;
using EventManager.Application.Requests.Notification;
using EventManager.Application.Responses.Notification;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class EventNotificationService : IEventNotificationService
{
    private readonly AppDbContext _appDbContext;

    public EventNotificationService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<bool> AttachNotification(EventNotificationRequest request, CancellationToken cancellationToken)
    {
        var eventExists = await _appDbContext.Events.AnyAsync(x => x.Id == request.EventId, cancellationToken);
        var notificationExists = await _appDbContext.EventNotifications.AnyAsync(x => x.NotificationId == request.NotificationId && x.EventId == request.EventId, cancellationToken);

        if (!eventExists || notificationExists)
        {
            return false;
        }

        var notification = new EventNotification { EventId = request.EventId, NotificationId = request.NotificationId };
        await _appDbContext.EventNotifications.AddAsync(notification, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> CreateNotification(CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = new Notification { DaysPriorStart = request.DaysPrior };

        await _appDbContext.Notifications.AddAsync(notification, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteNotification(int notificationId, CancellationToken cancellationToken)
    {
        var result = await _appDbContext.Notifications
                                .Where(x => x.Id == notificationId)
                                .ExecuteDeleteAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> DetachNotification(EventNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _appDbContext.EventNotifications
                                    .Where(x => x.NotificationId == request.NotificationId && x.EventId == request.EventId)
                                    .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }

    public async Task<IList<EventNotificationResponse>> GetEventNotifications(CancellationToken cancellationToken)
    {
        return await _appDbContext.Notifications
                                            .Include(x => x.EventNotifications)
                                                .ThenInclude(x => x.Event)
                                            .Select(x => new EventNotificationResponse(x))
                                            .ToListAsync(cancellationToken);
    }
}