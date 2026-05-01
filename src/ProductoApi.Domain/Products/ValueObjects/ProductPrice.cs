using ProductoApi.Domain.Exceptions;

namespace ProductoApi.Domain.Products.ValueObjects;

public sealed record ProductPrice
{
    public decimal Value { get; }

    public ProductPrice(decimal value)
    {
        if (value < 0)
            throw new DomainException("Product price cannot be negative.");

        Value = value;
    }

    public override string ToString() => Value.ToString("F2");
}
