namespace RepoRanger.Domain.Common;

public abstract class BaseCreatedAuditableEntity<TId> : BaseEntity<TId>, ICreatedAuditableEntity
{
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
}