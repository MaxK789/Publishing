using FluentValidation;
using Publishing.AppLayer.Commands;

namespace Publishing.AppLayer.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Pages).GreaterThan(0).WithMessage("Invalid page count");
            RuleFor(x => x.Tirage).GreaterThan(0).WithMessage("Invalid tirage");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
            RuleFor(x => x.PersonId).NotEmpty();
            RuleFor(x => x.Printery).NotEmpty().WithMessage("Printery is required");
        }
    }
}
