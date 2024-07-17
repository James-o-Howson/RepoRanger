﻿using RepoRanger.Application.DependencyInstances.Common;

namespace RepoRanger.Application.Projects.Common;

public sealed record ProjectVm(Guid Id, string Type, string Name, string Version, string Path, IEnumerable<DependencyInstanceVm> DependencyInstances);