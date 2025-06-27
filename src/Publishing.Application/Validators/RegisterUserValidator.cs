using FluentValidation;
using Publishing.Core.DTOs;

namespace Publishing.AppLayer.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(IValidator<string> emailValidator)
        {
            RuleFor(x => x.Email).SetValidator(emailValidator);
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
