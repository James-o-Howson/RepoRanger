namespace RepoRanger.Domain.Common;

public abstract class BaseModifiedAuditableEntity<TId> : BaseEntity<TId>, IModifiedAuditableEntity
{
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}