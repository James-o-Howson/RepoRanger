namespace RepoRanger.Abstractions.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);