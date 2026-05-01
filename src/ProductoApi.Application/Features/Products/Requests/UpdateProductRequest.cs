namespace ProductoApi.Application.Features.Products.Requests;

public sealed record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    decimal Price
);
