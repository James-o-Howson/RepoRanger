using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Source;

public class Repository : BaseAuditableEntity<Guid>
{
    private readonly List<Branch> _branches = [];
    
    private Repository() { }

    public Repository(string name, string remoteUrl)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(remoteUrl);
        
        Id = Guid.NewGuid();
        Name = name;
        RemoteUrl = remoteUrl;
    }

    public string Name { get; private set; }
    public string RemoteUrl { get; private set; }
    public Guid SourceId { get; private set; }
    public Guid DefaultBranchId { get; private set; }
    public IReadOnlyCollection<Branch> Branches => _branches;

    public void AddBranches(IList<Branch> branches)
    {
        ArgumentNullException.ThrowIfNull(branches);
        
        var defaults = branches.Where(b => b.IsDefault).ToList();
        switch (defaults.Count)
        {
            case > 1: 
                throw new ArgumentException("Cannot set more than 1 default branch per repository", nameof(branches));
            case 1:
                SetDefaultBranch(defaults.Single());
                break;
        }
        
        _branches.AddRange(branches);
    }
    
    private void SetDefaultBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        ArgumentNullException.ThrowIfNull(branch.Id);
        if (Branches.All(b => b.Id != branch.Id))
        {
            _branches.Add(branch);
        }

        DefaultBranchId = branch.Id;
    }
}