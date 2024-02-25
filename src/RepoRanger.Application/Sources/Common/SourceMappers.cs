﻿using RepoRanger.Application.Repositories.Common;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Sources.Common;

public static class SourceMappers
{
    public static Source ToEntity(this SourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var source = new Source(dto.Name, dto.Location);
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

        return new SourceDto(source.Name, source.Location, source.Repositories.ToDtos());
    }
}