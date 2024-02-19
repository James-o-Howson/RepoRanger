﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;

namespace RepoRanger.Application.Sources.Queries.GetByName;

public sealed record GetByNameQuery : IRequest<SourcePreviewDto?>
{
    public string Name { get; init; } = string.Empty;
};

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