using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("EventSubscriptions")]
public class EventSubscription
{
    public int EventId { get; set; }
    public Event Event { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
