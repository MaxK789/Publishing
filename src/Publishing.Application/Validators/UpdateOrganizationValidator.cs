using FluentValidation;
using Publishing.Core.DTOs;

namespace Publishing.AppLayer.Validators
{
    public class UpdateOrganizationValidator : AbstractValidator<UpdateOrganizationDto>
    {
        public UpdateOrganizationValidator(IValidator<string> emailValidator, PhoneFaxValidator phoneFaxValidator)
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).SetValidator(emailValidator);
            RuleFor(x => x.Phone).SetValidator(phoneFaxValidator);
            RuleFor(x => x.Fax).SetValidator(phoneFaxValidator);
        }
    }
}
