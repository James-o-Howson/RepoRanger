using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Persistence.Configuration;

internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Data)
            .IsRequired();
        
        builder.Property(m => m.Processed)
            .IsRequired();
        
        builder.Property(m => m.RetryCount)
            .IsRequired();

        builder.Property(m => m.Status)
            .IsRequired();
    }
}