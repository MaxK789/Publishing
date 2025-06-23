using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Commands;
using Publishing.AppLayer.Handlers;
using Publishing.AppLayer.Validators;
using Publishing.Core.Domain;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using FluentValidation;
using Publishing.Services;
using Publishing.Core.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class CreateOrderHandlerTests
    {
        private class StubOrderRepository : IOrderRepository
        {
            public Order? SavedOrder { get; private set; }
            public Task SaveAsync(Order order)
            {
                SavedOrder = order;
                return Task.CompletedTask;
            }
            public Task UpdateExpiredAsync() => Task.CompletedTask;
            public Task<DataTable> GetActiveAsync() => Task.FromResult(new DataTable());
            public Task<DataTable> GetByPersonAsync(string personId) => Task.FromResult(new DataTable());
            public Task<DataTable> GetAllAsync() => Task.FromResult(new DataTable());
            public Task DeleteAsync(int id) => Task.CompletedTask;
        }

        private class StubPrinteryRepository : IPrinteryRepository
        {
            public decimal PricePerPage { get; set; } = 2.5m;
            public int PagesPerDay { get; set; } = 100;
            public decimal GetPricePerPage() => PricePerPage;
            public int GetPagesPerDay() => PagesPerDay;
        }

        private class StubDateTimeProvider : IDateTimeProvider
        {
            public DateTime Today { get; set; } = new DateTime(2020,1,1);
        }

        private class StubUnitOfWork : IUnitOfWork
        {
            public Task BeginAsync() => Task.CompletedTask;
            public Task CommitAsync() => Task.CompletedTask;
            public Task RollbackAsync() => Task.CompletedTask;
            public IDbConnection Connection => new FakeDbConnection();
            public IDbTransaction Transaction => new FakeDbTransaction();
            private class FakeDbConnection : IDbConnection
            {
                public string? ConnectionString { get; set; }
                public int ConnectionTimeout => 0;
                public string Database => string.Empty;
                public ConnectionState State => ConnectionState.Open;
                public IDbTransaction BeginTransaction() => throw new NotImplementedException();
                public IDbTransaction BeginTransaction(IsolationLevel il) => throw new NotImplementedException();
                public void ChangeDatabase(string databaseName) { }
                public void Close() { }
                public IDbCommand CreateCommand() => throw new NotImplementedException();
                public void Open() { }
                public void Dispose() { }
            }
            private class FakeDbTransaction : IDbTransaction
            {
                public IDbConnection Connection => new FakeDbConnection();
                public IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;
                public void Commit() { }
                public void Rollback() { }
                public void Dispose() { }
            }
        }

        private class StubOrderEventsPublisher : IOrderEventsPublisher
        {
            public event Action<OrderDto>? OrderCreated;
            public event Action<OrderDto>? OrderUpdated;
            public void PublishOrderCreated(OrderDto order) => OrderCreated?.Invoke(order);
            public void PublishOrderUpdated(OrderDto order) => OrderUpdated?.Invoke(order);
        }

        private class StubNotifier : IUiNotifier
        {
            public void NotifyInfo(string message) { }
            public void NotifyWarning(string message) { }
            public void NotifyError(string message, string? details = null) { }
        }

        [TestMethod]
        public async Task Handle_ValidCommand_ReturnsOrder()
        {
            var repo = new StubOrderRepository();
            var printery = new StubPrinteryRepository();
            var handler = new CreateOrderHandler(repo, printery,
                new PriceCalculator(new StandardDiscountPolicy()),
                new CreateOrderCommandValidator(),
                new StubDateTimeProvider(),
                new StubUnitOfWork(),
                new StubOrderEventsPublisher(),
                new StubNotifier());
            var cmd = new CreateOrderCommand("book","Intro",10,3,"P1","PR");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.IsNotNull(repo.SavedOrder);
            Assert.AreEqual(cmd.Name, result.Name);
            Assert.AreEqual(75m, result.Price);
        }
    }
}
