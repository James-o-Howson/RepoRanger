﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepoRanger.Domain.Messages;

namespace RepoRanger.Data.Configuration;

internal sealed class MessageConfiguration : AuditableEntityConfiguration<Message>
{
    public override void Configure(EntityTypeBuilder<Message> builder)
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
        
        base.Configure(builder);
    }
}