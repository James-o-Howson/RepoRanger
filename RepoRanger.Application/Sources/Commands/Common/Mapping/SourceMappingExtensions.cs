using RepoRanger.Application.Sources.Commands.Common.Models;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Commands.Common.Mapping;

public static class SourceMappingExtensions
{
    public static Source ToEntity(this SourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var source = new Source(dto.Name);
        source.AddRepositories(dto.Repositories.ToEntities());
        return source;
    }
    
    public static IEnumerable<SourceDto> ToDtos(this IEnumerable<Source> sources)
    {
        ArgumentNullException.ThrowIfNull(sources);
        return sources.Select(ToDto);
    }
    
    public static SourceDto ToDto(this Source source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new SourceDto(source.Name, source.Repositories.ToDtos());
    }
}