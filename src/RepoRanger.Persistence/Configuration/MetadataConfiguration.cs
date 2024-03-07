using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class MetadataConfiguration : IEntityTypeConfiguration<Metadata>
{
    public void Configure(EntityTypeBuilder<Metadata> builder)
    {
        builder.HasKey(e => e.Id);
    }
}