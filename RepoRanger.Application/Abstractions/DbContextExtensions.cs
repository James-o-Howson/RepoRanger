using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Abstractions;

internal static class DbContextExtensions
{
    public static void RemoveEntity(this IApplicationDbContext context, Source source)
    {
        //todo: delete this.
        context.Dependencies.RemoveRange(source.Dependencies());
        context.Projects.RemoveRange(source.Projects());
        context.Branches.RemoveRange(source.Branches());
        context.Repositories.RemoveRange(source.Repositories);
        context.Sources.Remove(source);
    }
}