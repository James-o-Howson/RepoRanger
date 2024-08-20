﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Domain.Common;

namespace RepoRanger.Data.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUser _user;
    private readonly TimeProvider _dateTime;

    public AuditableEntitySaveChangesInterceptor(IUser user, TimeProvider dateTime)
    {
        _user = user;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        HandleCreatedEntities(context);
        HandleModifiedEntities(context);
    }
    
    private void HandleCreatedEntities(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (!entry.IsCreated()) continue;
            
            entry.Entity.CreatedBy = _user.UserId;
            entry.Entity.Created = _dateTime.GetUtcNow();
        }
    }

    private void HandleModifiedEntities(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (!entry.IsModified()) continue;
            
            entry.Entity.LastModifiedBy = _user.UserId;
            entry.Entity.LastModified = _dateTime.GetUtcNow();
        }
    }
}