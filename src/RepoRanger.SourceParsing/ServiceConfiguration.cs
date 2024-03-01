using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoRanger.Domain.Sources;
using RepoRanger.SourceParsing.Angular;
using RepoRanger.SourceParsing.Common.Configuration;
using RepoRanger.SourceParsing.Common.Options;
using RepoRanger.SourceParsing.DotNet;
using RepoRanger.SourceParsing.DotNet.Projects;
using RepoRanger.SourceParsing.Services;
using ProjectReferenceAttributeParser = RepoRanger.SourceParsing.DotNet.Projects.ProjectReferenceAttributeParser;

namespace RepoRanger.SourceParsing;

public static class ServiceConfiguration
{
    
}