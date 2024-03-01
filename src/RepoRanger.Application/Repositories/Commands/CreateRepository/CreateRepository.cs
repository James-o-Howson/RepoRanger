using MediatR;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Repositories.Commands.CreateRepository;

public sealed record CreateRepositoryCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string RemoteUrl { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public int SourceId { get; init; }
}

internal sealed class CreateRepositoryCommandHandler : IRequestHandler<CreateRepositoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateRepositoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository = Repository.Create(request.Name, request.RemoteUrl, request.BranchName, request.SourceId);

        await _context.Repositories.AddAsync(repository, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return repository.Id;
    }
}