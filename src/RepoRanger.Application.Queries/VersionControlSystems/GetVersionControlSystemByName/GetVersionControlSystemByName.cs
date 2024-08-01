using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Contracts.VersionControlSystems;

namespace RepoRanger.Application.Queries.VersionControlSystems.GetVersionControlSystemByName;

public sealed record GetVersionControlSystemByNameQuery : IRequest<VersionControlSystemPreviewDto?>
{
    public string Name { get; init; } = string.Empty;
};

internal sealed class GetVersionControlSystemByNameQueryHandler : IRequestHandler<GetVersionControlSystemByNameQuery, VersionControlSystemPreviewDto?>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetVersionControlSystemByNameQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<VersionControlSystemPreviewDto?> Handle(GetVersionControlSystemByNameQuery request, CancellationToken cancellationToken)
    {
        var versionControlSystem = await _applicationDbContext.VersionControlSystems
            .FirstOrDefaultAsync(s => s.Name == request.Name, cancellationToken);

        if (versionControlSystem is null) return null;

        return new VersionControlSystemPreviewDto
        {
            Id = versionControlSystem.Id,
            Name = versionControlSystem.Name,
            Location = versionControlSystem.Location
        };
    }
}