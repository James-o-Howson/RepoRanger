using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.OutboxMessages.ValueObjects;

namespace RepoRanger.Data.Configuration.OutboxMessages;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(v => v.Id)
            .HasConversion(id => id.Value,
                value => new OutboxMessageId(value))
            .ValueGeneratedNever();
        
        builder.ComplexProperty(m => m.Data)
            .IsRequired();

        builder.ComplexProperty(m => m.EventType)
            .IsRequired();
        
        builder.Property(m => m.RetryCount)
            .IsRequired();
        
        
        builder.Property(v => v.Created)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();

        builder.Property(v => v.ProcessingStatus)
            .IsRequired();
    }
}