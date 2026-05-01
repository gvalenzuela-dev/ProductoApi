namespace ProductoApi.Application.Features.Products.Requests;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    decimal Price
);
