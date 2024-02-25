namespace RepoRanger.Domain.Common;

public interface ICreatedAuditableEntity
{
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
}