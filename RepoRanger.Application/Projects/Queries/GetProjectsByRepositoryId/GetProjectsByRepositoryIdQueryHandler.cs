using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Exceptions;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryId;

public sealed record GetProjectsByRepositoryIdQuery : IRequest<ProjectsVm>
{
    public Guid RepositoryId { get; set; }
    public bool DefaultBranchOnly { get; set; }
}

internal sealed class GetProjectsByRepositoryIdQueryHandler : IRequestHandler<GetProjectsByRepositoryIdQuery, ProjectsVm>
{
    private readonly IApplicationDbContext _context;

    public GetProjectsByRepositoryIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectsVm> Handle(GetProjectsByRepositoryIdQuery request, CancellationToken cancellationToken)
    {
        var repository = await _context.Repositories
            .AsNoTracking()
            .Include(p => p.Branches)
            .ThenInclude(b => b.Projects)
            .FirstOrDefaultAsync(r => r.Id == request.RepositoryId, cancellationToken);

        if (repository is null) throw new NotFoundException($"Could not find repository for id: ${request.RepositoryId}");

        var projects = request.DefaultBranchOnly ? 
            repository.DefaultBranch.Projects : 
            repository.Branches.SelectMany(b => b.Projects);

        return new ProjectsVm
        {
            Projects = projects.Select(p => new ProjectVm
            {
                Id = p.Id,
                Name = p.Name,
                Version = p.Version
            }).ToList()
        };
    }
}