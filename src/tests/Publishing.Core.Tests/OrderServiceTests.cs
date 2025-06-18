using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Services;
using System;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        [TestMethod]
        public void CreateOrder_ReturnsFilledOrder()
        {
            var service = new OrderService();
            var dto = new OrderDto { Type = "book", Name = "Intro", Pages = 10, Tirage = 3 };

            var order = service.CreateOrder(dto);

            Assert.AreEqual(dto.Type, order.Type);
            Assert.AreEqual(dto.Name, order.Name);
            Assert.AreEqual(dto.Pages, order.Pages);
            Assert.AreEqual(dto.Tirage, order.Tirage);
            Assert.AreEqual("в роботі", order.Status);
            Assert.AreEqual(75m, order.Price);
            Assert.AreEqual(DateTime.Today.AddDays(1).Date, order.DateStart.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateOrder_InvalidPages_Throws()
        {
            var service = new OrderService();
            var dto = new OrderDto { Pages = -1, Tirage = 1 };
            service.CreateOrder(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateOrder_NullDto_Throws()
        {
            var service = new OrderService();
            service.CreateOrder(null);
        }

        [TestMethod]
        public void CreateOrder_RespectsCustomPricePerPage()
        {
            PriceCalculator.PricePerPage = 3m;
            var service = new OrderService();
            var dto = new OrderDto { Pages = 2, Tirage = 2 };

            var order = service.CreateOrder(dto);

            Assert.AreEqual(12m, order.Price);
            PriceCalculator.PricePerPage = 2.5m;
        }
    }
}
