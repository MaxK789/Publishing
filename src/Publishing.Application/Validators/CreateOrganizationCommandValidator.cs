using FluentValidation;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Validators;

public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationCommandValidator(IValidator<string> emailValidator, PhoneFaxValidator phoneFaxValidator)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).SetValidator(emailValidator);
        RuleFor(x => x.Phone).SetValidator(phoneFaxValidator);
        RuleFor(x => x.Fax).SetValidator(phoneFaxValidator);
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.PersonId).NotEmpty();
    }
}
