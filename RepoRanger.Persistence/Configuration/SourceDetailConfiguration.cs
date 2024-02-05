using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Application.Abstractions.Models;

namespace RepoRanger.Persistence.Configuration;

internal sealed class SourceDetailConfiguration : IEntityTypeConfiguration<SourceDetail> 
{
    public void Configure(EntityTypeBuilder<SourceDetail> builder)
    {
        builder.ToView(nameof(SourceDetail))
            .HasKey(s => s.Id);
    }
}