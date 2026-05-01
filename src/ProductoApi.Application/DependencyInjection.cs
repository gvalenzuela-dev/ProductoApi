using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductoApi.Application.Features.Products;
using ProductoApi.Application.Features.Products.Requests;
using ProductoApi.Application.Features.Products.Validators;

namespace ProductoApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        //services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        services.AddScoped<IValidator<CreateProductRequest>, CreateProductRequestValidator>();
        services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductRequestValidator>();
        return services;
    }
}
