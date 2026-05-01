using ProductoApi.Domain.Products;

namespace ProductoApi.Tests.Unit.Domain;

public class ProductIdTests
{
    [Fact]
    public void New_ShouldCreateProductIdWithUniqueGuid()
    {
        // Act
        var productId = ProductId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, productId.Value);
    }

    [Fact]
    public void New_ShouldCreateDifferentGuidsOnEachCall()
    {
        // Act
        var productId1 = ProductId.New();
        var productId2 = ProductId.New();

        // Assert
        Assert.NotEqual(productId1.Value, productId2.Value);
    }

    [Fact]
    public void Constructor_ShouldCreateProductIdWithGivenGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var productId = new ProductId(guid);

        // Assert
        Assert.Equal(guid, productId.Value);
    }

    [Fact]
    public void Constructor_ShouldAcceptEmptyGuid()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act
        var productId = new ProductId(emptyGuid);

        // Assert
        Assert.Equal(emptyGuid, productId.Value);
    }

    [Fact]
    public void ToString_ShouldReturnGuidAsString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var productId = new ProductId(guid);

        // Act
        var result = productId.ToString();

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void Equality_ShouldCompareByValue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var productId1 = new ProductId(guid);
        var productId2 = new ProductId(guid);

        // Act & Assert
        Assert.Equal(productId1, productId2);
    }

    [Fact]
    public void Inequality_ShouldNotBeEqualWhenValuesAreDifferent()
    {
        // Arrange
        var productId1 = new ProductId(Guid.NewGuid());
        var productId2 = new ProductId(Guid.NewGuid());

        // Act & Assert
        Assert.NotEqual(productId1, productId2);
    }
}
