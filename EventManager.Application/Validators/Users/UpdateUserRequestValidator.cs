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
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.FullName)
            .MinimumLength(3)
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.UserId)
            .NotNull();
    }
}
