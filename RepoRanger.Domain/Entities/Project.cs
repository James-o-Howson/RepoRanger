using RepoRanger.Domain.Common;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class Project : BaseAuditableEntity<Guid>
{
    public Guid RepositoryId { get; set; }
    public IList<Branch> Branches { get; private set; } = new List<Branch>();
    public IList<Dependency> Dependencies { get; private set; } = new List<Dependency>();

    public ProjectType Type { get; set; }
    public string Name { get; set; }
}