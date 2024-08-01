using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Dependencies.Entities;

namespace RepoRanger.Persistence.Configuration.Dependencies;

internal sealed class DependencySourceConfiguration : IEntityTypeConfiguration<DependencySource>
{
    public void Configure(EntityTypeBuilder<DependencySource> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.Name).IsUnique();
    }
}