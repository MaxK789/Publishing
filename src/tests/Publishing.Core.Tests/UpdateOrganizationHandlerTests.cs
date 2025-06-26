using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Handlers;
using Publishing.Core.Commands;
using Publishing.Core.Interfaces;
using FluentValidation;
using MediatR;
using Publishing.Services;
using Publishing.Core.DTOs;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Publishing.Core.Tests;

[TestClass]
public class UpdateOrganizationHandlerTests
{
    private class StubRepo : IOrganizationRepository
    {
        public UpdateOrganizationCommand? Updated;
        public CreateOrganizationCommand? Created;
        public string? ExistingName;
        public Task<string?> GetNameIfExistsAsync(string name) => Task.FromResult(name == ExistingName ? ExistingName : null);
        public Task InsertAsync(CreateOrganizationCommand cmd) { Created = cmd; return Task.CompletedTask; }
        public Task UpdateAsync(UpdateOrganizationCommand cmd) { Updated = cmd; return Task.CompletedTask; }

        public Task<OrganizationDto?> GetByPersonIdAsync(string personId) => Task.FromResult<OrganizationDto?>(null);
        public Task<IEnumerable<OrganizationDto>> GetAllAsync() => Task.FromResult<IEnumerable<OrganizationDto>>(Array.Empty<OrganizationDto>());
        public Task DeleteAsync(string id) => Task.CompletedTask;
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
            [AllowNull]
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

    private class StubEvents : IOrganizationEventsPublisher
    {
        public event Action<OrganizationDto>? OrganizationUpdated;

        public void PublishOrganizationUpdated(OrganizationDto organization)
        {
            OrganizationUpdated?.Invoke(organization);
        }
    }

    [TestMethod]
    public async Task Handle_ExistingName_Updates()
    {
        var repo = new StubRepo { ExistingName = "org" };
        var uow = new StubUnitOfWork();
        var handler = new UpdateOrganizationHandler(repo, new InlineValidator<UpdateOrganizationCommand>(), uow, new StubNotifier(), new StubEvents());
        var cmd = new UpdateOrganizationCommand { Id = "1", Name = "org" };
        await handler.Handle(cmd, CancellationToken.None);
        Assert.IsNotNull(repo.Updated);
        Assert.IsNull(repo.Created);
    }

    [TestMethod]
    public async Task Handle_NewName_Inserts()
    {
        var repo = new StubRepo { ExistingName = "other" };
        var uow = new StubUnitOfWork();
        var handler = new UpdateOrganizationHandler(repo, new InlineValidator<UpdateOrganizationCommand>(), uow, new StubNotifier(), new StubEvents());
        var cmd = new UpdateOrganizationCommand { Id = "2", Name = "org" };
        await handler.Handle(cmd, CancellationToken.None);
        Assert.IsNotNull(repo.Created);
        Assert.IsNull(repo.Updated);
    }
}
