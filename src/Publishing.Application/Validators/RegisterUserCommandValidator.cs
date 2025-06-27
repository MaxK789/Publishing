using FluentValidation;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IValidator<string> emailValidator)
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).SetValidator(emailValidator);
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
