using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.Common;

public class Auditable : ICreatedAuditableEntity
{
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
}