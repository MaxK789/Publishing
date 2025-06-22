using Publishing.Core.DTOs;
using FluentValidation;

namespace Publishing.Core.Interfaces
{
    public interface IOrderInputValidator
    {
        void Validate(CreateOrderDto dto);
    }
}
