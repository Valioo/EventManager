using EventManager.Application.Requests.Authentication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Validators.Auth;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(r => r.Email).NotNull().EmailAddress();
        RuleFor(r => r.Password).NotNull().MinimumLength(3);
    }
}
