using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Branch : BaseAuditableEntity<Guid>
{
    public Guid RepositoryId { get; set; }
    public IList<Project> Projects { get; private set; } = new List<Project>();
    public string Name { get; set; }
}