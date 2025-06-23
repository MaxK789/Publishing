using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using Publishing.Core.Commands;
using Publishing.Core.Interfaces;
using System.Resources;

namespace Publishing.AppLayer.Handlers;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand>
{
    private readonly IProfileRepository _repo;
    private readonly IValidator<UpdateProfileCommand> _validator;
    private readonly IUnitOfWork _uow;
    private readonly IUiNotifier _notifier;
    private readonly ResourceManager _resources = new("Publishing.Services.Resources.Notifications", typeof(UpdateProfileHandler).Assembly);

    public UpdateProfileHandler(IProfileRepository repo, IValidator<UpdateProfileCommand> validator, IUnitOfWork uow, IUiNotifier notifier)
    {
        _repo = repo;
        _validator = validator;
        _uow = uow;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        await _uow.BeginAsync();
        try
        {
            if (!string.IsNullOrEmpty(request.Email) && await _repo.EmailExistsAsync(request.Email).ConfigureAwait(false))
            {
                throw new InvalidOperationException("Email вже використовується");
            }
            await _repo.UpdateAsync(request).ConfigureAwait(false);
            await _uow.CommitAsync();
            _notifier.NotifyInfo(_resources.GetString("ProfileUpdated") ?? "Profile updated");
            return Unit.Value;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
