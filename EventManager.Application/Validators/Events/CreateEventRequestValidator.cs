using EventManager.Application.Requests.Events;
using FluentValidation;

namespace EventManager.Application.Validators.Events;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.StartDate)
            .NotNull()
            .LessThan(x => x.EndDate);

        RuleFor(x => x.EndDate)
            .NotNull()
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.CategoryId)
            .NotNull();

        RuleFor(x => x.LocationId)
            .NotNull();
    }
}
