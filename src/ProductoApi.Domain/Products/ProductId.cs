namespace ProductoApi.Domain.Products;

public sealed record ProductId(Guid Value)
{
    public static ProductId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
