using MediatR;
using RepoRanger.Abstractions.Exceptions;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Commands.Repositories.DeleteRepository;

public sealed record DeleteRepositoryCommand : IRequest
{
    public Guid RepositoryId { get; init; }
}

internal sealed class DeleteRepositoryCommandHandler : IRequestHandler<DeleteRepositoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteRepositoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteRepositoryCommand request, CancellationToken cancellationToken)
    {
        var id = new RepositoryId(request.RepositoryId);
        
        var repository = await _context.Repositories
            .FindAsync([id], cancellationToken);
        
        if(repository == null) throw new NotFoundException($"Cannot find Repository for Id = {request.RepositoryId}");
            
        repository.Delete();

        _context.Repositories.RemoveRange(repository);
        await _context.SaveChangesAsync(cancellationToken);
    }
}