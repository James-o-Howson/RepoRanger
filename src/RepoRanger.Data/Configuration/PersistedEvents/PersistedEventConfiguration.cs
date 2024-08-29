using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.PersistedEvents;
using RepoRanger.Domain.PersistedEvents.ValueObjects;

namespace RepoRanger.Data.Configuration.PersistedEvents;

internal sealed class PersistedEventConfiguration : IEntityTypeConfiguration<PersistedEvent>
{
    public void Configure(EntityTypeBuilder<PersistedEvent> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(v => v.Id)
            .HasConversion(id => id.Value,
                value => new PersistedEventId(value))
            .ValueGeneratedNever();
        
        builder.Property(m => m.Data)
            
            .IsRequired();
        
        builder.Property(m => m.EventType)
            .IsRequired();
        
        builder.Property(m => m.RetryCount)
            .IsRequired();

        builder.Property(m => m.Status)
            .IsRequired();
        
        builder.Property(v => v.Created)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode();
    }
}