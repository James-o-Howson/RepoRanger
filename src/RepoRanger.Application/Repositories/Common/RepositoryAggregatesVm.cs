﻿namespace RepoRanger.Application.Repositories.Common;

public sealed record RepositoryAggregatesVm(IReadOnlyCollection<RepositoryAggregateVm> RepositoryAggregates);