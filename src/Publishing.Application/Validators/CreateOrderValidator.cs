using FluentValidation;
using Publishing.AppLayer.Commands;

namespace Publishing.AppLayer.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Pages).GreaterThan(0);
            RuleFor(x => x.Tirage).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.PersonId).NotEmpty();
            RuleFor(x => x.Printery).NotEmpty();
        }
    }
}
