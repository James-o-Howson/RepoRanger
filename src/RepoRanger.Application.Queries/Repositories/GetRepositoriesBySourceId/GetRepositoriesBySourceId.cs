using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Contracts.Repositories;

namespace RepoRanger.Application.Queries.Repositories.GetRepositoriesBySourceId;

public sealed record GetRepositoriesBySourceIdQuery : IRequest<RepositorySummariesVm>
{
    public Guid? SourceId { get; init; }
}

internal sealed class GetRepositoriesBySourceIdQueryHandler : IRequestHandler<GetRepositoriesBySourceIdQuery, RepositorySummariesVm>
{
    private readonly IApplicationDbContext _context;

    public GetRepositoriesBySourceIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositorySummariesVm> Handle(GetRepositoriesBySourceIdQuery request, CancellationToken cancellationToken)
    {
        var repositories = await _context.Repositories
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.DefaultBranch)
            .Where(r => r.VersionControlSystemId == request.SourceId)
            .Select(r => new RepositorySummaryVm
            {
                Id = r.Id,
                Name = r.Name,
                RemoteUrl = r.RemoteUrl,
                DefaultBranchName = r.DefaultBranch,
                ParseTime = r.Created

            }).ToListAsync(cancellationToken);
        
        return new RepositorySummariesVm
        {
            RepositorySummaries = repositories
        };
    }
}