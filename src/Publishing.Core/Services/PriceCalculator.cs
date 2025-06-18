using System;

using Publishing.Core.Interfaces;

namespace Publishing.Core.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        public decimal Calculate(int pages, int copies, decimal pricePerPage)
        {
            if (pages < 0 || copies < 0)
                throw new ArgumentException("Values cannot be negative");

            decimal dPages = pages;
            decimal dCopies = copies;

            checked
            {
                decimal result = pricePerPage * dPages;
                result = result * dCopies;
                return result;
            }
        }
    }
}
