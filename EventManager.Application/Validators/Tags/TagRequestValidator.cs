using EventManager.Application.Requests.Tags;
using FluentValidation;

namespace EventManager.Application.Validators.Tags;

public class TagRequestValidator : AbstractValidator<TagRequest>
{
    public TagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);
    }
}