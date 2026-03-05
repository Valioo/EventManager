using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Events")]
public class Event : BaseEntity
{
    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int LocationId { get; set; }
    public Location Location { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Navigation
    public ICollection<TicketType> TicketTypes { get; set; }
    public ICollection<EventTag> EventTags { get; set; }
    public ICollection<EventSubscription> EventSubscribers { get; set; }
    public ICollection<EventNotification> EventNotifications { get; set; }

    public Event()
    {
        
    }

    public Event(string title, string description, DateTime startDate, DateTime endDate, int categoryId, int locationId)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        CategoryId = categoryId;
        LocationId = locationId;
    }
}