using FluentValidation;
using Publishing.Core.DTOs;

namespace Publishing.AppLayer.Validators;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderDto>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
