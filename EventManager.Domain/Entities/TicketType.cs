using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("TicketTypes")]
public class TicketType
{
    public int Id { get; set; }

    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Capacity { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; }

    // Navigation
    public ICollection<Ticket> Tickets { get; set; }
}
