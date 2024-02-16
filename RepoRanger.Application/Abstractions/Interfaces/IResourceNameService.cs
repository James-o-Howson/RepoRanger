﻿namespace RepoRanger.Application.Abstractions.Interfaces;

public interface IResourceNameService
{
    string GetOrphanedDependenciesResourceName { get; }
    string GetOrphanedProjectsResourceName { get; }
}