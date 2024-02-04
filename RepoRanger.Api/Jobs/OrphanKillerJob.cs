using Microsoft.EntityFrameworkCore;
using Quartz;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Persistence;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class OrphanKillerJob : IJob
{
    private readonly IApplicationDbContext _context;

    public OrphanKillerJob(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await DeleteOrphanedDependenciesAsync(context);
        await DeleteOrphanedProjectsAsync(context);
    }
    
    private async Task DeleteOrphanedProjectsAsync(IJobExecutionContext context)
    {
        var orphanedProjects = _context.Projects
            .FromSql($"""
                      SELECT *
                      FROM Projects
                      WHERE Id NOT IN (SELECT ProjectsId FROM DependencyProject)
                      AND Id NOT IN (SELECT ProjectsId FROM BranchProject)
                      """).ToList();
        
        _context.Projects.RemoveRange(orphanedProjects);

        await _context.SaveChangesAsync(context.CancellationToken);
    }

    private async Task DeleteOrphanedDependenciesAsync(IJobExecutionContext context)
    {
        var orphanedDependencies = _context.Dependencies
            .FromSql($"""
                      SELECT *
                      FROM Dependencies
                      WHERE Id NOT IN (SELECT DependenciesId FROM DependencyProject)
                      """).ToList();
        
        _context.Dependencies.RemoveRange(orphanedDependencies);

        await _context.SaveChangesAsync(context.CancellationToken);
    }
}