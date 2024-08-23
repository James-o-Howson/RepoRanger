using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using RepoRanger.Domain.Dependencies.Entities;

namespace RepoRanger.Domain.Common.Exceptions;

internal sealed class DomainException(string message)
    : Exception(message)
{
    public static void ThrowIfNullOrEmpty<T>([NotNull] IEnumerable<T>? arguments,
        [CallerArgumentExpression(nameof(arguments))] string? paramName = null)
    {
        // ReSharper disable PossibleMultipleEnumeration
        if (arguments is null || !arguments.Any())
        {
            ThrowNullOrEmptyException(arguments, paramName);
        }
        // ReSharper restore PossibleMultipleEnumeration
    }
    
    public static void ThrowIfNullOrEmpty([NotNull] string? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            ThrowNullOrEmptyException(argument, paramName);
        }
    }
    
    public static void ThrowIfNull([NotNull] object? argument, 
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }
    
    [DoesNotReturn]
    private static void ThrowNullOrEmptyException<T>(IEnumerable<T>? arguments, string? paramName)
    {
        ThrowIfNull(arguments, paramName);
        throw new DomainException($"{paramName} is empty");
    }
    
    [DoesNotReturn]
    private static void ThrowNullOrEmptyException(string? argument, string? paramName)
    {
        ThrowIfNull(argument, paramName);
        throw new DomainException($"{paramName} is empty");
    }
    
    [DoesNotReturn]
    private static void Throw(string? paramName) =>
        throw new DomainException(paramName ?? string.Empty);
}