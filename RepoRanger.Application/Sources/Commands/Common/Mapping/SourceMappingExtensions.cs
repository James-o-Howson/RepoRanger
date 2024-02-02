using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Commands.Common.Mapping;

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