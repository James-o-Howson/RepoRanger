using MediatR;
using RepoRanger.Abstractions.Exceptions;
using RepoRanger.Abstractions.Interfaces.Persistence;
using RepoRanger.Contracts.Repositories;

namespace RepoRanger.Queries.Repositories.GetRepositoryById;

public sealed record GetRepositoryByIdQuery(int Id) : IRequest<RepositorySummaryVm>;

internal sealed class GetRepositoryByIdRequestHandler : IRequestHandler<GetRepositoryByIdQuery, RepositorySummaryVm>
{
    private readonly IApplicationDbContext _context;

    public GetRepositoryByIdRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositorySummaryVm> Handle(GetRepositoryByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = await _context.Repositories.FindAsync([request.Id], cancellationToken);

        if (repository is null) throw new NotFoundException($"Unable to find Repository for Id = {request.Id}");

        return new RepositorySummaryVm
        {
            Id = repository.Id.Value,
            Name = repository.Name,
            RemoteUrl = repository.RemoteUrl,
            DefaultBranchName = repository.DefaultBranch,
            ParseTime = repository.Created
        };
    }
}