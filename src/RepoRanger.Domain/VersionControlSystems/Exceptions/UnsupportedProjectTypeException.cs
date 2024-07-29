﻿namespace RepoRanger.Domain.VersionControlSystems.Exceptions;

internal sealed class UnsupportedProjectTypeException(string value)
    : Exception($"Project Type \"{value}\" is unsupported.");