using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Exceptions;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Application.Commands.Sources.DeleteSourceCommand;

public sealed record DeleteSourceCommand : IRequest
{
    public Guid Id { get; init; }
} 

internal sealed class DeleteSourceCommandHandler : IRequestHandler<DeleteSourceCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IResourceNameService _resourceNameService;
    private readonly ISqlFileExecutorService _sqlFileExecutorService;

    public DeleteSourceCommandHandler(IApplicationDbContext context, IResourceNameService resourceNameService, ISqlFileExecutorService sqlFileExecutorService)
    {
        _context = context;
        _resourceNameService = resourceNameService;
        _sqlFileExecutorService = sqlFileExecutorService;
    }

    public async Task Handle(DeleteSourceCommand request, CancellationToken cancellationToken)
    {
        await DeleteSourceAsync(request, cancellationToken);
        await DeleteOrphanedDependenciesAsync(cancellationToken);
    }

    private async Task DeleteSourceAsync(DeleteSourceCommand request, CancellationToken cancellationToken)
    {
        var source = await _context.VersionControlSystems
            .Include(s => s.Repositories)
                .ThenInclude(r => r.DefaultBranch)
            .Include(s => s.Repositories)
                .ThenInclude(b => b.Projects)
                .ThenInclude(p => p.ProjectDependencies)
            .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (source is null) throw new NotFoundException($"Delete Failed - Unable find Source with Id: {request.Id}.");
        
        source.Delete();
        _context.VersionControlSystems.Remove(source);
        await _context.SaveChangesAsync(cancellationToken);
    }
    private async Task DeleteOrphanedDependenciesAsync(CancellationToken cancellationToken)
    {
        var orphanedDependencies = _sqlFileExecutorService
            .ExecuteEmbeddedResource<DependencyInstance>(_resourceNameService.GetOrphanedDependenciesResourceName)
            .ToList();
        
        _context.ProjectDependencies.RemoveRange(orphanedDependencies);
    
        await _context.SaveChangesAsync(cancellationToken);
    }
}