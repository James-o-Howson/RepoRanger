namespace RepoRanger.Domain.Exceptions;

internal sealed class UnsupportedSourceException(string code) : Exception($"Source \"{code}\" is unsupported.");