﻿namespace RepoRanger.Application.Projects.ViewModels;

public sealed class ProjectVm
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public int DependencyCount { get; init; }
    public int RepositoryId { get; init; }
    public string RepositoryName { get; set; } = string.Empty;
}