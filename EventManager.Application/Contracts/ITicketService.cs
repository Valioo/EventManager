using EventManager.Application.Requests.Tickets;
using EventManager.Application.Responses.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Contracts;

public interface ITicketService
{
    public Task<bool> PurchaseTicket(PurchaseTicketRequest request, CancellationToken cancellationToken);
    public Task<IList<TicketResponse>> GetMyTickets(CancellationToken cancellationToken);
}