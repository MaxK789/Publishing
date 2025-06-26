using FluentValidation;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Validators;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
