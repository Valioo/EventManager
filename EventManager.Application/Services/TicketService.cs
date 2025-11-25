using EventManager.Application.Contracts;
using EventManager.Application.Requests.Tickets;
using EventManager.Application.Responses.Tickets;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _appDbContext;
    private readonly ICurrentUserService _currentUser;

    public TicketService(AppDbContext appDbContext, ICurrentUserService currentUser)
    {
        _appDbContext = appDbContext;
        _currentUser = currentUser;
    }
    public async Task<IList<TicketResponse>> GetMyTickets(CancellationToken cancellationToken)
    {
        return await _appDbContext.Tickets
                            .Include(x => x.TicketType)
                                .ThenInclude(x => x.Event)
                            .Where(x => x.UserId == _currentUser.UserId)
                            .Select(x => new TicketResponse(x))
                            .ToListAsync(cancellationToken);
    }

    public async Task<bool> PurchaseTicket(PurchaseTicketRequest request, CancellationToken cancellationToken)
    {
        var ticketType = await _appDbContext.TicketTypes
                                .Include(x => x.Tickets)
                                .FirstOrDefaultAsync(x => x.Id == request.TicketTypeId, cancellationToken);

        if (ticketType is null || ticketType.Capacity <= ticketType.Tickets.Count)
        {
            return false;
        }

        var ticket = new Ticket(request.TicketTypeId, _currentUser.UserId!.Value);

        await _appDbContext.Tickets.AddAsync(ticket, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}