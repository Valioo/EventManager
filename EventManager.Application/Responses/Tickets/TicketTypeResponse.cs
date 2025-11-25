using EventManager.Domain.Entities;

namespace EventManager.Application.Responses.Tickets;

public class TicketTypeResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Capacity { get; set; }

    public TicketTypeResponse()
    {
        
    }

    public TicketTypeResponse(TicketType ticketType)
    {
        Id = ticketType.Id;
        Name = ticketType.Name;
        Price = ticketType.Price;
        Capacity = ticketType.Capacity;
    }
}