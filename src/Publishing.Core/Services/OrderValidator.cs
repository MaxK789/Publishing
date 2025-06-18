using System;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;

namespace Publishing.Core.Services
{
    public class OrderValidator : IOrderValidator
    {
        public void Validate(CreateOrderDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (dto.Pages <= 0)
                throw new ArgumentException("Pages must be positive");
            if (dto.Tirage <= 0)
                throw new ArgumentException("Tirage must be positive");
        }
    }
}
