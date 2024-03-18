using MediatR;
using RepoRanger.Application.Common.Exceptions;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Repositories.ViewModels;

namespace RepoRanger.Application.Repositories.Queries.GetRepositoryById;

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
            Id = repository.Id,
            Name = repository.Name,
            RemoteUrl = repository.RemoteUrl,
            DefaultBranchName = repository.DefaultBranch,
            ParseTime = repository.Created
        };
    }
}