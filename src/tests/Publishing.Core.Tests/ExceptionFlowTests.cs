using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Services;
using System;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class ExceptionFlowTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PriceCalculator_ThrowsOnNegative()
        {
            PriceCalculator.CalculateTotal(-5, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderService_ThrowsOnNullDto()
        {
            var service = new OrderService();
            service.CreateOrder(null);
        }
    }
}
