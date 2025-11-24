using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Events")]
public class Event
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

    // Navigation
    public ICollection<TicketType> TicketTypes { get; set; }
    public ICollection<EventTag> EventTags { get; set; }
    public ICollection<EventParticipant> EventParticipants { get; set; }
}