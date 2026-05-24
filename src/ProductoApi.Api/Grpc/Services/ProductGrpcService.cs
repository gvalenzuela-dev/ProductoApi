using Grpc.Core;
using ProductoApi.Application.Features.Products;
using ProductoApi.Application.Features.Products.Responses;

namespace ProductoApi.Api.Grpc.Services;

public class ProductGrpcService(IProductService productService) : ProductGrpc.ProductGrpcBase
{
    public override async Task<ProductListResponse> GetAll(
        Empty request,
        ServerCallContext context)
    {
        var response = await productService.GetAllAsync();

        return new ProductListResponse
        {
            Products =
            {
                response.Data?.Select(p => new ProductResponse
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    Price = (double)p.Price
                }) ?? Enumerable.Empty<ProductResponse>()
            }
        };
    }
    public override async Task<ProductResponse> GetById(
    GetByIdRequest request,
    ServerCallContext context)
    {
        var response = await productService.GetByIdAsync(Guid.Parse(request.Id));

        if (response.Data is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Producto no encontrado"));

        return new ProductResponse
        {
            Id = response.Data.Id.ToString(),
            Name = response.Data.Name,
            Description = response.Data.Description,
            Price = (double)response.Data.Price
        };
    }
    public override async Task<CreateProductResponse> Create(
    CreateProductRequest request,
    ServerCallContext context)
    {
        var requestData = new Application.Features.Products.Requests.CreateProductRequest
        (request.Name, request.Description, (decimal)request.Price);

        var response = await productService.CreateAsync(requestData);

        return new CreateProductResponse
        {
            Id = response.Data.ToString()
        };
    }
    public override async Task<BoolResponse> Update(
    UpdateProductRequest request,
    ServerCallContext context)
    {
        var response = await productService.UpdateAsync(new Application.Features.Products.Requests.UpdateProductRequest
        (
            Guid.Parse(request.Id),
            request.Name,
            request.Description,
            (decimal)request.Price
        ));

        return new BoolResponse
        {
            Success = response.Data
        };
    }
    public override async Task<BoolResponse> Delete(
        DeleteProductRequest request,
        ServerCallContext context)
    {
        var response = await productService.DeleteAsync(Guid.Parse(request.Id));

        return new BoolResponse
        {
            Success = response.Data
        };
    }
}