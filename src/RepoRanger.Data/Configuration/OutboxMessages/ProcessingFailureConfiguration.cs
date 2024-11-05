using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.OutboxMessages.Entities;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Ids;

namespace RepoRanger.Data.Configuration.OutboxMessages;

internal sealed class ProcessingFailureConfiguration : IEntityTypeConfiguration<ProcessingFailure>
{
    public void Configure(EntityTypeBuilder<ProcessingFailure> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(id => id.Value,
                value => new ProcessingFailureId(value))
            .ValueGeneratedNever();
        
        builder.ComplexProperty(m => m.Error)
            .IsRequired();
        
        builder.Property(m => m.OccuredAt)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
    }
}