using System.Runtime.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public enum Ecosystem
{
    [EnumMember(Value = "Go")]
    Go,
    [EnumMember(Value = "npm")]
    Npm,
    [EnumMember(Value = "OSS-Fuzz")]
    OssFuzz,
    [EnumMember(Value = "PyPI")]
    PyPI,
    [EnumMember(Value = "RubyGems")]
    RubyGems,
    [EnumMember(Value = "crates.io")]
    CratesIo,
    [EnumMember(Value = "Packagist")]
    Packagist,
    [EnumMember(Value = "Maven")]
    Maven,
    [EnumMember(Value = "NuGet")]
    NuGet,
    [EnumMember(Value = "Linux")]
    Linux,
    [EnumMember(Value = "Debian")]
    Debian,
    [EnumMember(Value = "Hex")]
    Hex,
    [EnumMember(Value = "Android")]
    Android,
}
