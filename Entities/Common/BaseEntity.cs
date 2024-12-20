﻿namespace Entities;

public interface IEntity
{
}
#nullable disable
public abstract class BaseEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}

public abstract class BaseEntity : BaseEntity<int>
{
}
