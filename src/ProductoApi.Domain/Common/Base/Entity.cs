namespace ProductoApi.Domain.Common.Base;

public class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
}
