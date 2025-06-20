using System;

using Publishing.Core.Interfaces;

namespace Publishing.Core.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        private readonly IDiscountPolicy _discountPolicy;

        public PriceCalculator(IDiscountPolicy discountPolicy)
        {
            _discountPolicy = discountPolicy ?? throw new ArgumentNullException(nameof(discountPolicy));
        }

        public decimal Calculate(int pages, int copies, decimal pricePerPage)
        {
            if (pages < 0)
                throw new ArgumentException("Value cannot be negative", nameof(pages));
            if (copies < 0)
                throw new ArgumentException("Value cannot be negative", nameof(copies));
            if (pricePerPage < 0)
                throw new ArgumentException("Value cannot be negative", nameof(pricePerPage));

            if (pages == 0 || copies == 0 || pricePerPage == 0m)
                return 0m;

            decimal dPages = pages;
            decimal dCopies = copies;

            checked
            {
                decimal result = pricePerPage * dPages;
                result = result * dCopies;
                return _discountPolicy.ApplyDiscount(pages, copies, result);
            }
        }
    }
}
