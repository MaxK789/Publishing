using System;

namespace Publishing.Core.Domain
{
    public class Printery
    {
        public string Name { get; set; } = string.Empty;
        public decimal PricePerPage { get; set; }
        public int PagesPerDay { get; set; }
    }
}
