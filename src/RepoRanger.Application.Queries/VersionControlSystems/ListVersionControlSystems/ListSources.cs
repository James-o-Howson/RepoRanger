using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Contracts.VersionControlSystems;

namespace RepoRanger.Application.Queries.VersionControlSystems.ListVersionControlSystems;

public sealed record ListVersionControlSystemsQuery : IRequest<VersionControlSystemsVm>;

internal sealed class ListSourcesQueryHandler : IRequestHandler<ListVersionControlSystemsQuery, VersionControlSystemsVm>
{
    private readonly IApplicationDbContext _context;

    public ListSourcesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VersionControlSystemsVm> Handle(ListVersionControlSystemsQuery request, CancellationToken cancellationToken)
    {
        return new VersionControlSystemsVm
        {
            VersionControlSystems = await _context.VersionControlSystems.AsNoTracking()
                .Select(s => new VersionControlSystemVm
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync(cancellationToken)
        };
    }
}