using EventManager.Application.Requests.Tickets;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Validators.Tickets;

public class CreateTicketTypeRequestValidator : AbstractValidator<CreateTicketTypeRequest>
{
    public CreateTicketTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(x => x.EventId)
            .NotNull();

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(0);
    }
}
