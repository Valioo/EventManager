using EventManager.Application.Contracts;
using EventManager.Application.Requests.Tickets;
using EventManager.Application.Responses.Tickets;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class TicketTypeService : ITicketTypeService
{
    private readonly AppDbContext _appDbContext;

    public TicketTypeService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<TicketTypeResponse> CreateTicketType(CreateTicketTypeRequest request, CancellationToken cancellationToken)
    {
        var evt = await _appDbContext.Events.FirstOrDefaultAsync(x => x.Id == request.EventId, cancellationToken);

        if (evt is null)
        {
            return null!;
        }

        var ticketType = new TicketType(request.Name, request.Price, request.Capacity, request.EventId);

        await _appDbContext.AddAsync(ticketType, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return new TicketTypeResponse(ticketType);
    }

    public async Task<IList<TicketTypeResponse>> GetTicketTypesByEvent(int eventId, CancellationToken cancellationToken)
    {
        return await _appDbContext.TicketTypes
                                    .Where(x => x.EventId == eventId)
                                    .Select(x => new TicketTypeResponse(x))
                                    .ToListAsync(cancellationToken);
    }

    public async Task<TicketTypeResponse> UpdateTicketType(UpdateTicketTypeRequest request, int ticketTypeId, CancellationToken cancellationToken)
    {
        var ticketType = await _appDbContext.TicketTypes.FirstOrDefaultAsync(x => x.Id == ticketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return null!;
        }

        ticketType.Capacity = request.Capacity;

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return new TicketTypeResponse(ticketType);
    }
}