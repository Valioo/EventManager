using EventManager.Application.Requests.Location;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Validators.Locations;

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(x => x.Address).NotEmpty().MinimumLength(3);
        RuleFor(x => x.City).NotEmpty().MinimumLength(3);
        RuleFor(x => x.VenueName).NotEmpty().MinimumLength(3);
    }
}
