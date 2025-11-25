using EventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Responses.Tickets;

public class TicketResponse
{
    public int TicketId { get; set; }
    public string TicketTypeName { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string EventName { get; set; }
    public int EventId { get; set; }
    public TicketResponse()
    {
        
    }
    public TicketResponse(Ticket ticket)
    {
        TicketId = ticket.Id;
        TicketTypeName = ticket.TicketType.Name;
        PurchaseDate = ticket.PurchaseDate;
        EventName = ticket.TicketType.Event.Title;
        EventId = ticket.TicketType.EventId;
    }
}