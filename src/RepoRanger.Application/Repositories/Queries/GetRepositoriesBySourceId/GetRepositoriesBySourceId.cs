using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Repositories.ViewModels;

namespace RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;

public sealed record GetRepositoriesBySourceIdQuery : IRequest<RepositoriesVm>
{
    public Guid? SourceId { get; init; }
}

internal sealed class GetRepositoriesBySourceIdQueryHandler : IRequestHandler<GetRepositoriesBySourceIdQuery, RepositoriesVm>
{
    private readonly IApplicationDbContext _context;

    public GetRepositoriesBySourceIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositoriesVm> Handle(GetRepositoriesBySourceIdQuery request, CancellationToken cancellationToken)
    {
        var repositories = await _context.Repositories
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.DefaultBranch)
            .Where(r => r.SourceId == request.SourceId)
            .Select(r => new RepositoryVm
            {
                Id = r.Id,
                Name = r.Name,
                RemoteUrl = r.RemoteUrl,
                DefaultBranchName = r.DefaultBranch,
                ParseTime = r.Created

            }).ToListAsync(cancellationToken);
        
        return new RepositoriesVm
        {
            Repositories = repositories
        };
    }
}