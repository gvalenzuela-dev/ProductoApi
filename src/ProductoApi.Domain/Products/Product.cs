using ProductoApi.Domain.Common.Base;
using ProductoApi.Domain.Products.ValueObjects;

namespace ProductoApi.Domain.Products;

public sealed class Product: Entity<ProductId>
{
    public ProductName Name { get; private set; }
    public ProductDescription Description { get; private set; }
    public ProductPrice Price { get; private set; }

    public Product(ProductId id, ProductName name, ProductDescription description, ProductPrice price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }

    public void Update(ProductName name, ProductDescription description, ProductPrice price)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}
