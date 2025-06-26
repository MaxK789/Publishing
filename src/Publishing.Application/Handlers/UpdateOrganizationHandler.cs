using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using Publishing.Core.Commands;
using Publishing.Core.Interfaces;
using Publishing.Services;
using System.Resources;

namespace Publishing.AppLayer.Handlers;

public class UpdateOrganizationHandler : IRequestHandler<UpdateOrganizationCommand, Unit>
{
    private readonly IOrganizationRepository _repo;
    private readonly IValidator<UpdateOrganizationCommand> _validator;
    private readonly IUnitOfWork _uow;
    private readonly IUiNotifier _notifier;
    private readonly IOrganizationEventsPublisher _events;
    private readonly ResourceManager _resources =
        new("Publishing.Services.Resources.Notifications", typeof(IUiNotifier).Assembly);

    public UpdateOrganizationHandler(IOrganizationRepository repo, IValidator<UpdateOrganizationCommand> validator, IUnitOfWork uow, IUiNotifier notifier, IOrganizationEventsPublisher events)
    {
        _repo = repo;
        _validator = validator;
        _uow = uow;
        _notifier = notifier;
        _events = events;
    }

    public async Task<Unit> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        await _uow.BeginAsync();
        try
        {
            string? existing = await _repo.GetNameIfExistsAsync(request.Name ?? string.Empty).ConfigureAwait(false);
            if (existing == request.Name)
            {
                await _repo.UpdateAsync(request).ConfigureAwait(false);
            }
            else
            {
                var createCmd = new CreateOrganizationCommand
                {
                    Name = request.Name ?? string.Empty,
                    Email = request.Email ?? string.Empty,
                    Phone = request.Phone ?? string.Empty,
                    Fax = request.Fax ?? string.Empty,
                    Address = request.Address ?? string.Empty,
                    PersonId = request.Id
                };
                await _repo.InsertAsync(createCmd).ConfigureAwait(false);
            }
            await _uow.CommitAsync();
            var org = await _repo.GetByPersonIdAsync(request.Id);
            if (org is not null)
                _events.PublishOrganizationUpdated(org);
            _notifier.NotifyInfo(_resources.GetString("OrganizationUpdated") ?? "Organization updated");
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
        return Unit.Value;
    }
}
