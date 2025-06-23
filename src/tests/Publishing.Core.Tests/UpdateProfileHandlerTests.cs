using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Handlers;
using Publishing.Core.Commands;
using Publishing.Core.Interfaces;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Publishing.Core.Tests;

[TestClass]
public class UpdateProfileHandlerTests
{
    private class StubRepo : IProfileRepository
    {
        public bool EmailExists;
        public UpdateProfileCommand? Updated;
        public Task<bool> EmailExistsAsync(string email) => Task.FromResult(EmailExists);
        public Task UpdateAsync(UpdateProfileCommand cmd) { Updated = cmd; return Task.CompletedTask; }
    }

    private class StubUnitOfWork : IUnitOfWork
    {
        public Task BeginAsync() => Task.CompletedTask;
        public Task CommitAsync() => Task.CompletedTask;
        public Task RollbackAsync() => Task.CompletedTask;
        public System.Data.IDbConnection Connection => new FakeDbConnection();
        public System.Data.IDbTransaction Transaction => new FakeDbTransaction();

        private class FakeDbConnection : System.Data.IDbConnection
        {
            public string ConnectionString { get; set; } = string.Empty;
            public int ConnectionTimeout => 0;
            public string Database => string.Empty;
            public System.Data.ConnectionState State => System.Data.ConnectionState.Open;
            public System.Data.IDbTransaction BeginTransaction() => throw new NotImplementedException();
            public System.Data.IDbTransaction BeginTransaction(System.Data.IsolationLevel il) => throw new NotImplementedException();
            public void ChangeDatabase(string databaseName) { }
            public void Close() { }
            public System.Data.IDbCommand CreateCommand() => throw new NotImplementedException();
            public void Open() { }
            public void Dispose() { }
        }

        private class FakeDbTransaction : System.Data.IDbTransaction
        {
            public System.Data.IDbConnection Connection => new FakeDbConnection();
            public System.Data.IsolationLevel IsolationLevel => System.Data.IsolationLevel.ReadCommitted;
            public void Commit() { }
            public void Rollback() { }
            public void Dispose() { }
        }
    }

    private class StubNotifier : IUiNotifier
    {
        public void NotifyInfo(string message) { }
        public void NotifyWarning(string message) { }
        public void NotifyError(string message, string? details = null) { }
    }

    [TestMethod]
    public async Task Handle_EmailNotExists_Updates()
    {
        var repo = new StubRepo();
        var uow = new StubUnitOfWork();
        var handler = new UpdateProfileHandler(repo, new InlineValidator<UpdateProfileCommand>(), uow, new StubNotifier());
        var cmd = new UpdateProfileCommand { Id = "1", Email = "e@e.com" };
        await handler.Handle(cmd, CancellationToken.None);
        Assert.IsNotNull(repo.Updated);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public async Task Handle_EmailExists_Throws()
    {
        var repo = new StubRepo { EmailExists = true };
        var uow = new StubUnitOfWork();
        var handler = new UpdateProfileHandler(repo, new InlineValidator<UpdateProfileCommand>(), uow, new StubNotifier());
        var cmd = new UpdateProfileCommand { Id = "1", Email = "e@e.com" };
        await handler.Handle(cmd, CancellationToken.None);
    }
}
