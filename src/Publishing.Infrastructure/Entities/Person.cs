using System;
using System.Collections.Generic;

namespace Publishing.Infrastructure.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Pass? Pass { get; set; }
    }
}
