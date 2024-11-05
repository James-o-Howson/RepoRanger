using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.OutboxMessages.Entities;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

namespace RepoRanger.Data.Configuration.OutboxMessages;

internal sealed class DeadLetterEntryConfiguration : IEntityTypeConfiguration<DeadLetterEntry>
{
    public void Configure(EntityTypeBuilder<DeadLetterEntry> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasConversion(id => id.Value,
                value => new DeadLetterEntryId(value))
            .ValueGeneratedNever();

        builder.HasOne(d => d.FinalProcessingFailure)
            .WithOne()
            .HasForeignKey<DeadLetterEntry>(d => d.FinalProcessingFailureId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(d => d.OccuredAt)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
    }
}