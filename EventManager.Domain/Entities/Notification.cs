using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Notifications")]
public class Notification
{
    public int Id { get; set; }
    public int DaysPriorStart { get; set; }

    public ICollection<EventNotification> EventNotifications { get; set; }
}