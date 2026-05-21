using ProductoApi.Application.Features.Products;
using ProductoApi.Application.Features.Products.Responses;

namespace ProductoApi.Api.GraphQL.Queries
{
    public class ProductQuery
    {
        [UseFiltering]
        [UseSorting]
        public async Task<List<GetProductResponse>> GetProducts(
                [Service] IProductService service)
        {
            var response = await service.GetAllAsync();
            return response.Data.ToList() ?? new List<GetProductResponse>();
        }

        public async Task<GetProductResponse?> GetProductById(
            Guid id,
            [Service] IProductService service)
        {
            var response = await service.GetByIdAsync(id);
            return response.Data;
        }
    }
}
