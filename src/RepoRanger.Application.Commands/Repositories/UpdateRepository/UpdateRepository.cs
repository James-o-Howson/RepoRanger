using MediatR;
using RepoRanger.Application.Abstractions.Exceptions;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Application.Commands.Repositories.UpdateRepository;

public sealed record UpdateRepositoryCommand : IRequest<Guid>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string RemoteUrl { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
}

internal sealed class UpdateRepositoryCommandHandler : IRequestHandler<UpdateRepositoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateRepositoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository = await _context.Repositories.FindAsync([request.Id], cancellationToken);

        if (repository is null) throw new NotFoundException($"Cannot find Repository for Id = {request.Id}");

        repository.Name = request.Name;
        repository.RemoteUrl = request.RemoteUrl;
        repository.DefaultBranch = request.BranchName;
            
        await _context.SaveChangesAsync(cancellationToken);

        return repository.Id.Value;
    }
}