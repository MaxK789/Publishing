using System;

namespace Publishing.Core.Services
{
    public static class PriceCalculator
    {
        public static decimal PricePerPage { get; set; } = 2.5m;

        public static decimal CalculateTotal(int pages, int copies)
        {
            if (pages < 0 || copies < 0)
                throw new ArgumentException("Values cannot be negative");

            // perform calculation in decimal with overflow checking
            decimal dPages = pages;
            decimal dCopies = copies;

            checked
            {
                decimal result = PricePerPage * dPages;
                result = result * dCopies;
                return result;
            }
        }
    }
}
