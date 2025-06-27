using FluentValidation;
using Publishing.Core.DTOs;

namespace Publishing.AppLayer.Validators
{
    public class UpdateOrganizationValidator : AbstractValidator<UpdateOrganizationDto>
    {
        public UpdateOrganizationValidator(IValidator<string> emailValidator, PhoneFaxValidator phoneFaxValidator)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Organization name is required");
            RuleFor(x => x.Email).SetValidator(emailValidator);
            RuleFor(x => x.Phone).SetValidator(phoneFaxValidator).WithMessage("Invalid phone format");
            RuleFor(x => x.Fax).SetValidator(phoneFaxValidator).WithMessage("Invalid fax format");
        }
    }
}
