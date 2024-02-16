using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Repositories.ViewModels;

namespace RepoRanger.Application.Repositories.Queries.ListRepositories;

public sealed record ListRepositoriesQuery : IRequest<RepositoriesVm>;

internal sealed class ListRepositoriesQueryHandler : IRequestHandler<ListRepositoriesQuery, RepositoriesVm>
{
    private readonly IApplicationDbContext _context;

    public ListRepositoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositoriesVm> Handle(ListRepositoriesQuery request, CancellationToken cancellationToken)
    {
        return new RepositoriesVm
        {
            Repositories = await _context.Repositories
                .Include(r => r.DefaultBranch)
                .AsNoTracking()
                .Select(r => new RepositoryVm
                {
                    Id = r.Id,
                    Name = r.Name,
                    RemoteUrl = r.RemoteUrl,
                    DefaultBranchId = r.DefaultBranchId,
                    DefaultBranchName = r.DefaultBranch.Name,
                    ParseTime = r.Created
                })
                .ToListAsync(cancellationToken)
        };
    }
}