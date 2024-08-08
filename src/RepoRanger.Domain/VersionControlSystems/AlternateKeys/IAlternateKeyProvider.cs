namespace RepoRanger.Domain.VersionControlSystems.AlternateKeys;

internal interface IAlternateKeyProvider
{
    AlternateKey GetAlternateKey { get; }
}