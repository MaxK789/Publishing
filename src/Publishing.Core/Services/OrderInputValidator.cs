using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;

namespace Publishing.Core.Services
{
    public class OrderInputValidator : AbstractValidator<CreateOrderDto>, IOrderInputValidator
    {
        public OrderInputValidator()
        {
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Printery).NotEmpty().WithMessage("Printery is required");
            RuleFor(x => x.PersonId).NotEmpty();
            RuleFor(x => x.Pages).GreaterThan(0).WithMessage("Invalid page count");
            RuleFor(x => x.Tirage).GreaterThan(0).WithMessage("Invalid tirage");
        }

        public new void Validate(CreateOrderDto dto)
        {
            var result = base.Validate(dto);
            if (!result.IsValid) throw new ValidationException(result.Errors);
        }
    }
}
