using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.DTOs;
using Publishing.Core.Domain;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private class StubOrderRepository : IOrderRepository
        {
            public Order? SavedOrder { get; private set; }
            public void Save(Order order) => SavedOrder = order;

            public Task UpdateExpiredAsync() => Task.CompletedTask;

            public Task<DataTable> GetActiveAsync() =>
                Task.FromResult(new DataTable());

            public Task<DataTable> GetByPersonAsync(string personId) =>
                Task.FromResult(new DataTable());

            public Task<DataTable> GetAllAsync() =>
                Task.FromResult(new DataTable());

            public Task DeleteAsync(int id) => Task.CompletedTask;
        }

        private class StubPrinteryRepository : IPrinteryRepository
        {
            public decimal PricePerPage { get; set; } = 2.5m;
            public int PagesPerDay { get; set; } = 100;
            public decimal GetPricePerPage() => PricePerPage;
            public int GetPagesPerDay() => PagesPerDay;
        }

        private class StubLogger : ILogger
        {
            public void LogInformation(string message) { }
            public void LogError(string message, Exception ex) { }
        }

        private class StubDateTimeProvider : IDateTimeProvider
        {
            public DateTime Today { get; set; } = new DateTime(2020, 1, 1);
        }

        private class PassThroughValidator : IOrderValidator
        {
            public void Validate(CreateOrderDto dto) { }
        }

        [TestMethod]
        public async Task CreateOrder_ReturnsFilledOrder()
        {
            var orderRepo = new StubOrderRepository();
            var printeryRepo = new StubPrinteryRepository();
            var service = new OrderService(
                orderRepo,
                printeryRepo,
                new StubLogger(),
                new PriceCalculator(),
                new PassThroughValidator(),
                new StubDateTimeProvider());
            var dto = new CreateOrderDto { Type = "book", Name = "Intro", Pages = 10, Tirage = 3 };

            var order = await service.CreateOrderAsync(dto);

            Assert.AreEqual(dto.Type, order.Type);
            Assert.AreEqual(dto.Name, order.Name);
            Assert.AreEqual(dto.Pages, order.Pages);
            Assert.AreEqual(dto.Tirage, order.Tirage);
            Assert.AreEqual(OrderStatus.InProgress, order.Status);
            Assert.AreEqual(75m, order.Price);
            Assert.AreEqual(new DateTime(2020,1,2).Date, order.DateStart.Date);
            Assert.IsNotNull(orderRepo.SavedOrder);
        }

        [TestMethod]
        public async Task CreateOrder_InvalidPages_Throws()
        {
            var orderRepo = new StubOrderRepository();
            var printeryRepo = new StubPrinteryRepository();
            var service = new OrderService(orderRepo, printeryRepo, new StubLogger(), new PriceCalculator(), new PassThroughValidator(), new StubDateTimeProvider());
            var dto = new CreateOrderDto { Pages = -1, Tirage = 1 };
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                await service.CreateOrderAsync(dto));
        }

        [TestMethod]
        public async Task CreateOrder_NullDto_Throws()
        {
            var orderRepo = new StubOrderRepository();
            var printeryRepo = new StubPrinteryRepository();
            var service = new OrderService(orderRepo, printeryRepo, new StubLogger(), new PriceCalculator(), new PassThroughValidator(), new StubDateTimeProvider());
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
                await service.CreateOrderAsync(null));
        }

        [TestMethod]
        public async Task CreateOrder_RespectsCustomPricePerPage()
        {
            var orderRepo = new StubOrderRepository();
            var printeryRepo = new StubPrinteryRepository { PricePerPage = 3m };
            var service = new OrderService(orderRepo, printeryRepo, new StubLogger(), new PriceCalculator(), new PassThroughValidator(), new StubDateTimeProvider());
            var dto = new CreateOrderDto { Pages = 2, Tirage = 2 };

            var order = await service.CreateOrderAsync(dto);

            Assert.AreEqual(12m, order.Price);
        }
    }
}
