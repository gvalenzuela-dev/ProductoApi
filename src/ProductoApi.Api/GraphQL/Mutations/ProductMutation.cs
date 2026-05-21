
using ProductoApi.Application.Features.Products;
using ProductoApi.Application.Features.Products.Requests;

namespace ProductoApi.Api.GraphQL.Mutations;

public class ProductMutation
{
    
    public async Task<Guid> CreateProduct(
        CreateProductRequest request,
        [Service] IProductService service)
    {
        var result = await service.CreateAsync(request);
        return result.Data;
    }

    public async Task<bool> UpdateProduct(
        UpdateProductRequest request,
        [Service] IProductService service)
    {
        var result = await service.UpdateAsync(request);
        return result.Data;
    }

    public async Task<bool> DeleteProduct(
        Guid id,
        [Service] IProductService service)
    {
        var result = await service.DeleteAsync(id);
        return result.Data;
    }

}
