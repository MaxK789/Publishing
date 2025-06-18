using System;

namespace Publishing.Core.Services
{
    public class OrderDto
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Pages { get; set; }
        public int Tirage { get; set; }
    }

    public class Order
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Pages { get; set; }
        public int Tirage { get; set; }
        public DateTime DateStart { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class OrderService
    {
        public Order CreateOrder(OrderDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (dto.Pages <= 0 || dto.Tirage <= 0)
                throw new ArgumentException("Pages and tirage must be positive");

            return new Order
            {
                Type = dto.Type,
                Name = dto.Name,
                Pages = dto.Pages,
                Tirage = dto.Tirage,
                DateStart = DateTime.Today.AddDays(1),
                Status = "в роботі",
                Price = PriceCalculator.CalculateTotal(dto.Pages, dto.Tirage)
            };
        }
    }
}
