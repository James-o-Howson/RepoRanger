﻿namespace RepoRanger.Application.Sources.Common.Models;

public sealed record RepositoryDto(string Name, string RemoteUrl, IEnumerable<BranchDto> Branches);