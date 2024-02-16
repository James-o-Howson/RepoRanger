using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Repository : BaseCreatedAuditableEntity<Guid>
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
    public Source Source { get; set; }
    public IReadOnlyCollection<Branch> Branches => _branches;
    public Guid DefaultBranchId { get; private set; }
    public Branch DefaultBranch { get; private set; }

    public IEnumerable<Dependency> Dependencies => Branches
        .SelectMany(b => b.Projects)
        .SelectMany(p => p.Dependencies)
        .ToList();

    public void AddBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        AddBranches(new List<Branch> { branch });
    }
    
    public void AddBranches(IList<Branch> branches)
    {
        ArgumentNullException.ThrowIfNull(branches);
        
        var defaults = branches.Where(b => b.IsDefault).ToList();
        if (defaults.Count > 1) throw new ArgumentException("Cannot set more than 1 default branch per repository", nameof(branches));

        _branches.AddRange(branches);
        if (defaults.Count == 1) TrySetDefaultBranch(defaults.Single());
    }
    
    private void TrySetDefaultBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        ArgumentNullException.ThrowIfNull(branch.Id);
        
        if (Branches.All(b => b.Id != branch.Id))
        {
            _branches.Add(branch);
        }

        DefaultBranchId = branch.Id;
        DefaultBranch = branch;
    }

    internal void Delete()
    {
        foreach (var branch in Branches)
        {
            branch.Delete();
        }

        _branches.Clear();
    }
}