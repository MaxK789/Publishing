using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using Publishing.Core.Commands;
using Publishing.Core.Interfaces;

namespace Publishing.AppLayer.Handlers;

public class UpdateOrganizationHandler : IRequestHandler<UpdateOrganizationCommand>
{
    private readonly IOrganizationRepository _repo;
    private readonly IValidator<UpdateOrganizationCommand> _validator;
    private readonly IUnitOfWork _uow;

    public UpdateOrganizationHandler(IOrganizationRepository repo, IValidator<UpdateOrganizationCommand> validator, IUnitOfWork uow)
    {
        _repo = repo;
        _validator = validator;
        _uow = uow;
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
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
        return Unit.Value;
    }
}
