using MediatR;
using RepoRanger.Application.Abstractions.Exceptions;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Application.Commands.Repositories.CreateRepository;

public sealed record CreateRepositoryCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string RemoteUrl { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public Guid VersionControlSystemId { get; init; }
}

internal sealed class CreateRepositoryCommandHandler : IRequestHandler<CreateRepositoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateRepositoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRepositoryCommand request, CancellationToken cancellationToken)
    {
        var versionControlSystem =
            await _context.VersionControlSystems.FindAsync([request.VersionControlSystemId], cancellationToken: cancellationToken);
        if (versionControlSystem is null)
            throw new NotFoundException($"Cannot find Version Control System with Id = {request.VersionControlSystemId}");

        var repository = Repository.Create(versionControlSystem, request.Name, request.RemoteUrl, request.BranchName);
        repository.VersionControlSystemId = new VersionControlSystemId(request.VersionControlSystemId);

        await _context.Repositories.AddAsync(repository, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return repository.Id.Value;
    }
}