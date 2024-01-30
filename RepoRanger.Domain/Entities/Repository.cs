using RepoRanger.Domain.Common;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Domain.Entities;

public class Repository : BaseAuditableEntity<Guid>
{
    public Guid DefaultBranchId { get; set; }

    public string Name { get; set; }
    public string Url { get; set; }
    public string RemoteUrl { get; set; }
    public Source Source { get; set; }
    
    public IList<Branch> Branches { get; private set; } = new List<Branch>();
}