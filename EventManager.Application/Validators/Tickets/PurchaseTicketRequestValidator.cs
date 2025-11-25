using EventManager.Application.Requests.Tickets;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Validators.Tickets;

public class PurchaseTicketRequestValidator : AbstractValidator<PurchaseTicketRequest>
{
    public PurchaseTicketRequestValidator()
    {
        RuleFor(x => x.TicketTypeId)
            .NotNull();
    }
}