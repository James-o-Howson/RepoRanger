﻿namespace RepoRanger.Domain.Common;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; }
}