using FluentValidation;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Validators;

public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
{
    public UpdateOrganizationCommandValidator(IValidator<string> emailValidator, PhoneFaxValidator phoneFaxValidator)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).SetValidator(emailValidator);
        RuleFor(x => x.Phone).SetValidator(phoneFaxValidator);
        RuleFor(x => x.Fax).SetValidator(phoneFaxValidator);
    }
}
