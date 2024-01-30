using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Dependency : BaseAuditableEntity<Guid>
{
    public IList<Project> Projects { get; private set; } = new List<Project>();
    
    public string Name { get; set; }
    public string Version { get; set; }
}