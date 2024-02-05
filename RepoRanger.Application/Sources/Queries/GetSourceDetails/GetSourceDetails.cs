using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Application.Sources.Queries.GetSourceDetails;

public sealed record GetSourceDetailsQuery : IRequest<SourceDetailsVm>;

internal sealed class GetSourceDetailsQueryHandler : IRequestHandler<GetSourceDetailsQuery, SourceDetailsVm>
{
    private readonly IApplicationDbContext _context;

    public GetSourceDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SourceDetailsVm> Handle(GetSourceDetailsQuery request, CancellationToken cancellationToken)
    {
        return new SourceDetailsVm
        {
            Sources = await _context.SourceDetails
                .Select(s => new SourceDetailVm
                {
                    Id = s.Id,
                    Name = s.Name,
                    ParseTime = s.ParseTime,
                    DefaultBranchId = s.DefaultBranchId,
                    DefaultBranchName = s.DefaultBranchName,
                    ProjectsCount = s.ProjectsCount,
                    DependenciesCount = s.DependenciesCount
                })
                .ToListAsync(cancellationToken)
        };
    }
}