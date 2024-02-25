using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Branch : ICreatedAuditableEntity
{
    private Branch() { }

    public Branch(string name, bool isDefault)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(isDefault);
        
        Id = Guid.NewGuid();
        Name = name;
        IsDefault = isDefault;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsDefault { get; private set; }
    public Guid RepositoryId { get; private set; }
    public Repository Repository { get; private set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
}