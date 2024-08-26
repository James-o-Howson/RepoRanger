using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.PersistedEvents;
using RepoRanger.Domain.PersistedEvents.ValueObjects;

namespace RepoRanger.Data.Configuration;

internal sealed class PersistedEventConfiguration : AuditableEntityConfiguration<PersistedEvent>
{
    public override void Configure(EntityTypeBuilder<PersistedEvent> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(v => v.Id)
            .HasConversion(id => id.Value,
                value => new PersistedEventId(value))
            .ValueGeneratedNever();
        
        builder.Property(m => m.Data)
            .IsRequired();
        
        builder.Property(m => m.Processed)
            .IsRequired();
        
        builder.Property(m => m.RetryCount)
            .IsRequired();

        builder.Property(m => m.Status)
            .IsRequired();
        
        base.Configure(builder);
    }
}