﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Contracts.Repositories;

namespace RepoRanger.Application.Queries.Repositories.GetRepositoriesByVersionControlSystemId;

public sealed record GetRepositoriesByVersionControlSystemIdQuery : IRequest<RepositorySummariesVm>
{
    public Guid? VersionControlSystemId { get; init; }
}

internal sealed class GetRepositoriesByVersionControlSystemIdQueryHandler : IRequestHandler<GetRepositoriesByVersionControlSystemIdQuery, RepositorySummariesVm>
{
    private readonly IApplicationDbContext _context;

    public GetRepositoriesByVersionControlSystemIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositorySummariesVm> Handle(GetRepositoriesByVersionControlSystemIdQuery request, CancellationToken cancellationToken)
    {
        var repositories = await _context.Repositories
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.DefaultBranch)
            .Where(r => r.VersionControlSystemId == request.VersionControlSystemId)
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