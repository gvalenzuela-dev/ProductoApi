using ProductoApi.Domain.Exceptions;

namespace ProductoApi.Domain.Products.ValueObjects;

public sealed record ProductDescription
{
    public string Value { get; }

    public ProductDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Product description cannot be empty.");
        if (value.Length > 500)
            throw new DomainException("Product description cannot exceed 500 characters.");

        Value = value;
    }

    public override string ToString() => Value;
}
