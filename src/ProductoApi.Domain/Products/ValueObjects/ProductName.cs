using System;
using ProductoApi.Domain.Exceptions;

namespace ProductoApi.Domain.Products.ValueObjects;

public sealed record ProductName
{
    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Product name cannot be empty.");
        if (value.Length > 100)
            throw new DomainException("Product name cannot exceed 100 characters.");

        Value = value;
    }

    public override string ToString() => Value;
}
