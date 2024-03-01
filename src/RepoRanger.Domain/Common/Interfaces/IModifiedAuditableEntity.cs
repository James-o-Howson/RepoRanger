namespace RepoRanger.Domain.Common.Interfaces;

public interface IModifiedAuditableEntity : ICreatedAuditableEntity
{
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}