using RepoRanger.Application.Sources.Common.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Common.Mapping;

internal static class SourceMappingExtensions
{
    public static Source ToEntity(this SourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var source = new Source(dto.Name);
        source.AddRepositories(dto.Repositories.ToEntities());
        return source;
    }
}