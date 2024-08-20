using System.Runtime.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public enum RangeType
{
    [EnumMember(Value = "UNSPECIFIED")]
    Unspecified,
    [EnumMember(Value = "GIT")]
    Git,
    [EnumMember(Value = "SEMVER")]
    Semver,
    [EnumMember(Value = "ECOSYSTEM")]
    Ecosystem,
}
