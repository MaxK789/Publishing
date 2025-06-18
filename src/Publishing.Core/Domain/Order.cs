using System;

namespace Publishing.Core.Domain
{
    public class Order
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Pages { get; set; }
        public int Tirage { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.InProgress;
        public decimal Price { get; set; }
    }
}
