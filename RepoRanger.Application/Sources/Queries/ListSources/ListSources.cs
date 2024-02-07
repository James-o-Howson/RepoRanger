using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Application.Sources.Queries.ListSources;

public sealed record ListSourcesQuery : IRequest<SourcesViewModel>;

internal sealed class ListSourcesQueryHandler : IRequestHandler<ListSourcesQuery, SourcesViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListSourcesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SourcesViewModel> Handle(ListSourcesQuery request, CancellationToken cancellationToken)
    {
        return new SourcesViewModel
        {
            Sources = await _context.Sources.AsNoTracking()
                .Select(s => new SourceViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync(cancellationToken)
        };
    }
}