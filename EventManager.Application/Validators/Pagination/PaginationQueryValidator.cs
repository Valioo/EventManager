using EventManager.Application.Helpers.Pagination;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Validators.Pagination;

public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
{
    public PaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber).NotNull().GreaterThan(0);
        RuleFor(x => x.PageSize).NotNull().GreaterThan(0).LessThanOrEqualTo(50);
    }
}
