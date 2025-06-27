using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;

namespace Publishing.Core.Services
{
    public class OrderInputValidator : AbstractValidator<CreateOrderDto>, IOrderInputValidator
    {
        public OrderInputValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Printery).NotEmpty();
            RuleFor(x => x.PersonId).NotEmpty();
            RuleFor(x => x.Pages).GreaterThan(0);
            RuleFor(x => x.Tirage).GreaterThan(0);
        }

        public new void Validate(CreateOrderDto dto)
        {
            var result = base.Validate(dto);
            if (!result.IsValid) throw new ValidationException(result.Errors);
        }
    }
}
