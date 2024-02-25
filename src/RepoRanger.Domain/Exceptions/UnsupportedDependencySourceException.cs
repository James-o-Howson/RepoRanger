namespace RepoRanger.Domain.Exceptions;

internal sealed class UnsupportedDependencySourceException(string value)
    : Exception($"Dependency Source \"{value}\" is unsupported.");