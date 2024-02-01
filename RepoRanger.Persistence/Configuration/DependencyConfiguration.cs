using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class DependencyConfiguration : GuidBaseAuditableEntityConfiguration<Dependency>
{
    public override void Configure(EntityTypeBuilder<Dependency> builder)
    {
        
    }
}