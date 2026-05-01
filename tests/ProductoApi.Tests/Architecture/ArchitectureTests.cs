using NetArchTest.Rules;
using ProductoApi.Application.Features.Products;
using ProductoApi.Domain.Products;

namespace ProductoApi.Tests.Architecture;

public class ArchitectureTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Other_Layers()
    {
        var result = Types.InAssembly(typeof(Product).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "ProductoApi.Infrastructure",
                "ProductoApi.Api",
                "ProductoApi.Application")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Api()
    {
        var result = Types.InAssembly(typeof(ProductService).Assembly)
            .ShouldNot()
            .HaveDependencyOn("ProductoApi.Api")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Api()
    {
        var result = Types.InAssembly(
                typeof(Infrastructure.Persistance.AppDbContext).Assembly)
            .ShouldNot()
            .HaveDependencyOn("ProductoApi.Api")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }    
}
