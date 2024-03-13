using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Repositories.ViewModels;

namespace RepoRanger.Application.Repositories.Queries.ListRepositories;

public sealed record ListRepositoriesQuery : IRequest<RepositorySummariesVm>;

internal sealed class ListRepositoriesQueryHandler : IRequestHandler<ListRepositoriesQuery, RepositorySummariesVm>
{
    private readonly IApplicationDbContext _context;

    public ListRepositoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositorySummariesVm> Handle(ListRepositoriesQuery request, CancellationToken cancellationToken)
    {
        return new RepositorySummariesVm
        {
            RepositorySummaries = await _context.Repositories
                .AsNoTracking()
                .Select(r => new RepositorySummaryVm
                {
                    Id = r.Id,
                    Name = r.Name,
                    RemoteUrl = r.RemoteUrl,
                    DefaultBranchName = r.DefaultBranch,
                    ParseTime = r.Created
                })
                .ToListAsync(cancellationToken)
        };
    }
}