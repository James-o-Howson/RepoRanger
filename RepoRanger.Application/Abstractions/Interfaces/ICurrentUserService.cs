namespace RepoRanger.Application.Abstractions.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}