using EventManager.Application.Requests.Categories;
using FluentValidation;

namespace EventManager.Application.Validators.Categories;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().MinimumLength(3);
    }
}
