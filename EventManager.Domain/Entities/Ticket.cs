using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Domain.Entities;

[Table("Tickets")]
public class Ticket
{
    public int Id { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    public int TicketTypeId { get; set; }
    public TicketType TicketType { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
