namespace RepoRanger.Domain.Common.Interfaces;

public interface ICreatedAuditableEntity
{
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
}