using FluentValidation;
using Publishing.Core.DTOs;

namespace Publishing.AppLayer.Validators
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileValidator(IValidator<string> emailValidator, PhoneFaxValidator phoneFaxValidator)
        {
            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email!).SetValidator(emailValidator);
            });
            RuleFor(x => x.Phone).SetValidator(phoneFaxValidator).WithMessage("Invalid phone format");
            RuleFor(x => x.Fax).SetValidator(phoneFaxValidator).WithMessage("Invalid fax format");
            RuleFor(x => x).Must(HasAnyValue).WithMessage("No data to update");
        }

        private static bool HasAnyValue(UpdateProfileDto dto)
        {
            return !string.IsNullOrEmpty(dto.FirstName) ||
                   !string.IsNullOrEmpty(dto.LastName) ||
                   !string.IsNullOrEmpty(dto.Email) ||
                   !string.IsNullOrEmpty(dto.Status) ||
                   !string.IsNullOrEmpty(dto.Phone) ||
                   !string.IsNullOrEmpty(dto.Fax) ||
                   !string.IsNullOrEmpty(dto.Address);
        }
    }
}
