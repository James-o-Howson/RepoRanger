using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;

public sealed record GetRepositoriesBySourceIdQuery : IRequest<RepositoriesVm>
{
    public Guid SourceId { get; set; }
}

internal sealed class GetRepositoriesBySourceIdQueryHandler : IRequestHandler<GetRepositoriesBySourceIdQuery,  RepositoriesVm>
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
            .Where(r => r.SourceId == request.SourceId)
            .Select(r => new RepositoryVm
            {
                Id = r.Id,
                Name = r.Name,
                RemoteUrl = r.RemoteUrl
            }).ToListAsync(cancellationToken);
        
        return new RepositoriesVm
        {
            Repositories = repositories
        };
    }
}