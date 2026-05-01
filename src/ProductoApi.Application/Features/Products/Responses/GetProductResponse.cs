namespace ProductoApi.Application.Features.Products.Responses;

public sealed record GetProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price
);
