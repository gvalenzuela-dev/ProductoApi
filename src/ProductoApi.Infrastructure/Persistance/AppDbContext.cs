using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductoApi.Domain.Products;
using ProductoApi.Domain.Products.ValueObjects;

namespace ProductoApi.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasConversion(
                v => v.Value,
                v => new ProductId(v))
                .Metadata.SetValueComparer(new ValueComparer<ProductId>(
                    (c1, c2) => c1.Value == c2.Value,
                    c => c.Value.GetHashCode(),
                    c => new ProductId(c.Value)));
            entity.Property(e => e.Name).HasConversion(
                v => v.Value,
                v => new ProductName(v));
            entity.Property(e => e.Description).HasConversion(
                v => v.Value,
                v => new ProductDescription(v));
            entity.Property(e => e.Price).HasConversion(
                v => v.Value,
                v => new ProductPrice(v));

            entity.HasData(
                new {
                    Id = new ProductId(new Guid("11111111-1111-1111-1111-111111111111")),
                    Name = new ProductName("Sample Product"),
                    Description = new ProductDescription("This is a sample product."),
                    Price = new ProductPrice(9.99m)
                },
                new {
                    Id = new ProductId(new Guid("22222222-2222-2222-2222-222222222222")),
                    Name = new ProductName("Another Product"),
                    Description = new ProductDescription("This is another sample product."),
                    Price = new ProductPrice(19.99m)
                },
                new {
                    Id = new ProductId(new Guid("33333333-3333-3333-3333-333333333333")),
                    Name = new ProductName("Third Product"),
                    Description = new ProductDescription("This is the third sample product."),
                    Price = new ProductPrice(29.99m)
                }
            );
        });
    }
}
