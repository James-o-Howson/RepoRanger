using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Messages;

namespace RepoRanger.Persistence.Configuration;

internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(v => v.Id)
            .HasConversion(id => id.Value,
                value => new MessageId(value))
            .ValueGeneratedNever();
        
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