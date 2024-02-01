using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Repository : BaseAuditableEntity<Guid>
{
    private readonly List<Branch> _branches = [];
    
    private Repository() { }

    public Repository(string name, string url, string remoteUrl)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(url);
        ArgumentException.ThrowIfNullOrEmpty(remoteUrl);
        
        Id = Guid.NewGuid();
        Name = name;
        Url = url;
        RemoteUrl = remoteUrl;
    }

    public string Name { get; private set; }
    public string Url { get; private set; }
    public string RemoteUrl { get; private set; }
    public Guid SourceId { get; private set; }
    public Guid DefaultBranchId { get; private set; }
    public IReadOnlyCollection<Branch> Branches => _branches;
    
    public void SetDefaultBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        ArgumentNullException.ThrowIfNull(branch.Id);
        if (Branches.All(b => b.Id != branch.Id))
        {
            _branches.Add(branch);
        }

        DefaultBranchId = branch.Id;
    }

    public void AddBranches(IEnumerable<Branch> branches)
    {
        ArgumentNullException.ThrowIfNull(branches);
        
        _branches.AddRange(branches);
    }
}