using FluentValidation;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IValidator<string> emailValidator)
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        RuleFor(x => x.Email).SetValidator(emailValidator);
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}
