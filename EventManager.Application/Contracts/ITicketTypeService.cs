using EventManager.Application.Requests.Tickets;
using EventManager.Application.Responses.Tickets;

namespace EventManager.Application.Contracts;

public interface ITicketTypeService
{
    public Task<IList<TicketTypeResponse>> GetTicketTypesByEvent(int eventId, CancellationToken cancellationToken);
    public Task<TicketTypeResponse> CreateTicketType(CreateTicketTypeRequest request, CancellationToken cancellationToken);
    public Task<TicketTypeResponse> UpdateTicketType(UpdateTicketTypeRequest request, int ticketTypeId, CancellationToken cancellationToken);
}