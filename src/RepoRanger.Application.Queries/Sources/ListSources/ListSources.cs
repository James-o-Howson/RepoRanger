using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Contracts.Sources;

namespace RepoRanger.Application.Queries.Sources.ListSources;

public sealed record ListSourcesQuery : IRequest<SourcesVm>;

internal sealed class ListSourcesQueryHandler : IRequestHandler<ListSourcesQuery, SourcesVm>
{
    private readonly IApplicationDbContext _context;

    public ListSourcesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SourcesVm> Handle(ListSourcesQuery request, CancellationToken cancellationToken)
    {
        return new SourcesVm
        {
            Sources = await _context.Sources.AsNoTracking()
                .Select(s => new SourceVm
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync(cancellationToken)
        };
    }
}