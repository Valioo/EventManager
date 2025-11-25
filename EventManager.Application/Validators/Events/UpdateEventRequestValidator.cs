using EventManager.Application.Requests.Events;
using FluentValidation;

namespace EventManager.Application.Validators.Events;

public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
{
    public UpdateEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .MinimumLength(3)
            .When(x => x.Title != null);

        RuleFor(x => x.Description)
            .NotNull()
            .MinimumLength(3)
            .When(x => x.Description != null);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .When(x => x.StartDate != null && x.EndDate != null);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate != null && x.StartDate != null);
    }
}
