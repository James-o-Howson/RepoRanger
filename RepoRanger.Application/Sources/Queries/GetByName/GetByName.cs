using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Application.Sources.Queries.GetByName;

public sealed record GetByNameQuery(string Name) : IRequest<SourcePreviewDto?>;

internal sealed class GetByNameQueryHandler : IRequestHandler<GetByNameQuery, SourcePreviewDto?>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetByNameQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<SourcePreviewDto?> Handle(GetByNameQuery request, CancellationToken cancellationToken)
    {
        var source = await _applicationDbContext.Sources
            .FirstOrDefaultAsync(s => s.Name == request.Name, cancellationToken);

        if (source is null) return null;

        return new SourcePreviewDto
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}