using EventManager.Application.Requests.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Validators.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().When(x => x is not null);
        RuleFor(x => x.FullName).MinimumLength(3).When(x => x is not null);
        RuleFor(x => x.UserId).NotNull();
    }
}
