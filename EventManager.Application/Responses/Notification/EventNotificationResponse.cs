using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Responses.Notification;

public class EventNotificationResponse
{
    public int DaysPrior { get; set; }
    public int NotificationId { get; set; }
    public List<int> EventIds { get; set; } = [];

    public EventNotificationResponse()
    {
        
    }

    public EventNotificationResponse(Domain.Entities.Notification notification)
    {
        NotificationId = notification.Id;
        DaysPrior = notification.DaysPriorStart;
        EventIds = [.. notification.EventNotifications.Select(x => x.EventId)];
    }
}