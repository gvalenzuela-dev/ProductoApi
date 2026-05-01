using ProductoApi.Application.Common.Responses;
using ProductoApi.Application.Features.Products.Requests;
using ProductoApi.Application.Features.Products.Responses;

namespace ProductoApi.Application.Features.Products;

public interface IProductService
{
    Task<Result<GetProductResponse?>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<GetProductResponse>>> GetAllAsync();
    Task<Result<Guid>> CreateAsync(CreateProductRequest request);
    Task<Result<bool>> UpdateAsync(UpdateProductRequest request);
    Task<Result<bool>> DeleteAsync(Guid id);
}
