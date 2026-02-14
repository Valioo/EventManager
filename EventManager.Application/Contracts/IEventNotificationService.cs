using EventManager.Application.Requests.Notification;
using EventManager.Application.Responses.Notification;

namespace EventManager.Application.Contracts;

public interface IEventNotificationService
{
    public Task<bool> CreateNotification(CreateNotificationRequest request, CancellationToken cancellationToken);
    public Task<bool> UpdateNotification(int notificationId, UpdateNotificationRequest request, CancellationToken cancellationToken);
    public Task<bool> DeleteNotification(int notificationId, CancellationToken cancellationToken);
    public Task<IList<EventNotificationResponse>> GetEventNotifications(CancellationToken cancellationToken);
    public Task<bool> AttachNotification(EventNotificationRequest request, CancellationToken cancellationToken);
    public Task<bool> DetachNotification(EventNotificationRequest request, CancellationToken cancellationToken);
}