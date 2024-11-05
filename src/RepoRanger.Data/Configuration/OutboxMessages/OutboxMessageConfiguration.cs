using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.OutboxMessages.Entities;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

namespace RepoRanger.Data.Configuration.OutboxMessages;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(id => id.Value,
                value => new OutboxMessageId(value))
            .ValueGeneratedNever();
        
        builder.ComplexProperty(m => m.Data)
            .IsRequired();

        builder.ComplexProperty(m => m.EventType)
            .IsRequired();
        
        builder.ComplexProperty(m => m.RetryPolicy)
            .IsRequired();
        
        builder.Property(m => m.Status)
            .IsRequired();

        // builder.OwnsOne(m => m.Metadata, metadata =>
        // {
        //     metadata.Property(m => m.RetryCount)
        //         .IsRequired();
        //     metadata.Property(m => m.LastProcessedAt);
        //     metadata.Property(m => m.NextRetryAt);
        // });
        
        builder.ComplexProperty(m => m.Metadata)
            .IsRequired();
        
        builder.HasMany(m => m.Failures)
            .WithOne(f => f.OutboxMessage)
            .HasForeignKey(f => f.OutboxMessageId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(m => m.DeadLetterEntry)
            .WithOne(d => d.FailedMessage)
            .HasForeignKey<DeadLetterEntry>(d => d.FailedMessageId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(m => m.CreatedAt)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
    }
}