using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.Core.DTOs;
using Publishing.Core.Domain;
using System;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class ExceptionFlowTests
    {
        private class StubOrderRepository : IOrderRepository
        {
            public void Save(Order order) { }

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
            public decimal GetPricePerPage() => 2.5m;
            public int GetPagesPerDay() => 100;
        }

        private class StubLogger : ILogger
        {
            public void LogInformation(string message) { }
            public void LogError(string message, Exception ex) { }
        }

        private class StubValidator : IOrderValidator
        {
            public void Validate(CreateOrderDto dto) { }
        }

        private class StubDateTimeProvider : IDateTimeProvider
        {
            public DateTime Today { get; set; } = DateTime.Today;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PriceCalculator_ThrowsOnNegative()
        {
            var calc = new PriceCalculator(new StandardDiscountPolicy());
            calc.Calculate(-5, 2, 2.5m);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderService_ThrowsOnNullDto()
        {
            var service = new OrderService(
                new StubOrderRepository(),
                new StubPrinteryRepository(),
                new StubLogger(),
                new PriceCalculator(new StandardDiscountPolicy()),
                new StubValidator(),
                new StubDateTimeProvider());
            service.CreateOrder(null);
        }
    }
}
