using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Domain.Entities;

[Table("EventsNotifications")]
public class EventNotification
{
    public int EventId { get; set; }
    public Event Event { get; set; }

    public int NotificationId { get; set; }
    public Notification Notification { get; set; }
}