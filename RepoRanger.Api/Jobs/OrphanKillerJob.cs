using Microsoft.EntityFrameworkCore;
using Quartz;
using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Api.Jobs;

[DisallowConcurrentExecution]
internal sealed class OrphanKillerJob : IJob
{
    internal static readonly JobKey JobKey = new(nameof(OrphanKillerJob));
    
    private readonly IApplicationDbContext _context;
    private readonly ILogger<OrphanKillerJob> _logger;

    public OrphanKillerJob(IApplicationDbContext context, ILogger<OrphanKillerJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await DeleteOrphanedDependenciesAsync(context);
            await DeleteOrphanedProjectsAsync(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while trying to delete orphaned records.");
            context.Result = e;
        }
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