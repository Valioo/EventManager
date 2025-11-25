using EventManager.Application.Requests.Location;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Validators.Locations;

public class UpdateLocationRequestValidator : AbstractValidator<UpdateLocationRequest>
{
    public UpdateLocationRequestValidator()
    {
        RuleFor(x => x.Address)
            .MinimumLength(3)
            .When(x => x.Address != null);

        RuleFor(x => x.City)
            .MinimumLength(3)
            .When(x => x.City != null);

        RuleFor(x => x.VenueName)
            .MinimumLength(3)
            .When(x => x.VenueName != null);
    }
}
