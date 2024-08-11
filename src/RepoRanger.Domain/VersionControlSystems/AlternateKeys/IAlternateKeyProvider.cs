namespace RepoRanger.Domain.VersionControlSystems.AlternateKeys;

public interface IAlternateKeyProvider
{
    AlternateKey GetAlternateKey { get; }
}