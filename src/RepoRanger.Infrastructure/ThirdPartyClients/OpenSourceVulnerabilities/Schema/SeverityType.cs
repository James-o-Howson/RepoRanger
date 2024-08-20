using System.Runtime.Serialization;

namespace RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

public enum SeverityType
{
    [EnumMember(Value = "UNSPECIFIED")]
    Unspecified = 0,

    [EnumMember(Value = "CVSS_V3")]
    CvssV3 = 1,

    [EnumMember(Value = "CVSS_V2")]
    CvssV2 = 2,
    
    [EnumMember(Value = "CVSS_V4")]
    CvssV4 = 3,
}
