using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductoApi.Application.Common.Responses;
using ProductoApi.Application.Features.Products.Requests;
using ProductoApi.Application.Features.Products.Responses;
using ProductoApi.Domain.Exceptions;
using ProductoApi.Domain.Products;
using ProductoApi.Domain.Products.ValueObjects;
using ProductoApi.Infrastructure.Persistance;

namespace ProductoApi.Application.Features.Products;

public class ProductService(AppDbContext context, IValidator<CreateProductRequest> createValidator, IValidator<UpdateProductRequest> updateValidator) : IProductService
{
    public async Task<Result<Guid>> CreateAsync(CreateProductRequest request)
    {
        var validationResult = await createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<Guid>.Fail([.. validationResult.Errors.Select(e => e.ErrorMessage)]);
        }
        try
        {
            var product = new Product(
                new ProductId(Guid.NewGuid()),
                new ProductName(request.Name),
                new ProductDescription(request.Description),
                new ProductPrice(request.Price));
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return Result<Guid>.Ok(product.Id.Value);
        }
        catch (DomainException ex)
        {
            return Result<Guid>.Fail(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var product = await context.Products
            .FindAsync(new ProductId(id));

        if (product is null)
            return Result<bool>.Fail("Product not found");

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return Result<bool>.Ok(true);
    }

    public async Task<Result<IEnumerable<GetProductResponse>>> GetAllAsync()
    {
        var products = await context.Products
            .Select(p => new GetProductResponse(
                p.Id.Value,
                p.Name.Value,
                p.Description.Value,
                p.Price.Value))
            .ToListAsync();
        return Result<IEnumerable<GetProductResponse>>.Ok(products);
    }

    public async Task<Result<GetProductResponse?>> GetByIdAsync(Guid id)
    {
        var product = await context.Products
        .FindAsync(new ProductId(id));

        if (product is null)
            return Result<GetProductResponse?>.Fail("Product not found");

        return Result<GetProductResponse?>.Ok(new GetProductResponse(
            product.Id.Value,
            product.Name.Value,
            product.Description.Value,
            product.Price.Value));
    }

    public async Task<Result<bool>> UpdateAsync(UpdateProductRequest request)
    {
        var validationResult = await updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<bool>.Fail([.. validationResult.Errors.Select(e => e.ErrorMessage)]);
        }

        var product = await context.Products
            .FindAsync(new ProductId(request.Id));

        if (product is null)
            return Result<bool>.Fail("Product not found");

        product.Update(
            new ProductName(request.Name),
            new ProductDescription(request.Description),
            new ProductPrice(request.Price));

        await context.SaveChangesAsync();
        return Result<bool>.Ok(true);
    }
}
