using FluentValidation;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Validators;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator(IValidator<string> emailValidator, PhoneFaxValidator phoneFaxValidator)
    {
        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email!).SetValidator(emailValidator);
        });
        RuleFor(x => x.Phone).SetValidator(phoneFaxValidator).WithMessage("Invalid phone format");
        RuleFor(x => x.Fax).SetValidator(phoneFaxValidator).WithMessage("Invalid fax format");
        RuleFor(x => x).Must(HasAnyValue).WithMessage("No data to update");
    }

    private static bool HasAnyValue(UpdateProfileCommand cmd)
    {
        return !string.IsNullOrEmpty(cmd.FirstName) ||
               !string.IsNullOrEmpty(cmd.LastName) ||
               !string.IsNullOrEmpty(cmd.Email) ||
               !string.IsNullOrEmpty(cmd.Status) ||
               !string.IsNullOrEmpty(cmd.Phone) ||
               !string.IsNullOrEmpty(cmd.Fax) ||
               !string.IsNullOrEmpty(cmd.Address);
    }
}
