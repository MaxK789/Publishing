using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using Publishing.Core.Commands;
using Publishing.Core.Interfaces;

namespace Publishing.AppLayer.Handlers;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand>
{
    private readonly IProfileRepository _repo;
    private readonly IValidator<UpdateProfileCommand> _validator;
    private readonly IUnitOfWork _uow;

    public UpdateProfileHandler(IProfileRepository repo, IValidator<UpdateProfileCommand> validator, IUnitOfWork uow)
    {
        _repo = repo;
        _validator = validator;
        _uow = uow;
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
            return Unit.Value;
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
