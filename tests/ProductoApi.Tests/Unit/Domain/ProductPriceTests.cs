using ProductoApi.Domain.Exceptions;
using ProductoApi.Domain.Products.ValueObjects;

namespace ProductoApi.Tests.Unit.Domain;

public class ProductPriceTests
{
    [Fact]
    public void Constructor_WithValidPositivePrice_ShouldCreateProductPrice()
    {
        // Arrange
        var price = 99.99m;

        // Act
        var productPrice = new ProductPrice(price);

        // Assert
        Assert.Equal(price, productPrice.Value);
    }

    [Fact]
    public void Constructor_WithZeroPrice_ShouldCreateProductPrice()
    {
        // Arrange
        var price = 0m;

        // Act
        var productPrice = new ProductPrice(price);

        // Assert
        Assert.Equal(price, productPrice.Value);
    }

    [Fact]
    public void Constructor_WithNegativePrice_ShouldThrowDomainException()
    {
        // Arrange
        var negativePrice = -10.50m;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductPrice(negativePrice));
        Assert.Equal("Product price cannot be negative.", exception.Message);
    }

    [Fact]
    public void Constructor_WithSmallNegativePrice_ShouldThrowDomainException()
    {
        // Arrange
        var negativePrice = -0.01m;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductPrice(negativePrice));
        Assert.Equal("Product price cannot be negative.", exception.Message);
    }

    [Fact]
    public void Constructor_WithLargePositivePrice_ShouldSucceed()
    {
        // Arrange
        var price = 999999999.99m;

        // Act
        var productPrice = new ProductPrice(price);

        // Assert
        Assert.Equal(price, productPrice.Value);
    }

    [Fact]
    public void Constructor_WithVerySmallPositivePrice_ShouldSucceed()
    {
        // Arrange
        var price = 0.01m;

        // Act
        var productPrice = new ProductPrice(price);

        // Assert
        Assert.Equal(price, productPrice.Value);
    }

    [Fact]
    public void ToString_ShouldReturnValueFormattedAsTwoDecimalPlaces()
    {
        // Arrange
        var price = 99.5m;
        var productPrice = new ProductPrice(price);

        // Act
        var result = productPrice.ToString();

        // Assert
        Assert.Equal("99,50", result);
    }

    [Fact]
    public void ToString_WithZeroPrice_ShouldReturnFormattedZero()
    {
        // Arrange
        var productPrice = new ProductPrice(0m);

        // Act
        var result = productPrice.ToString();

        // Assert
        Assert.Equal("0,00", result);
    }

    [Fact]
    public void ToString_WithWholeNumber_ShouldIncludeTwoDecimalPlaces()
    {
        // Arrange
        var productPrice = new ProductPrice(100m);

        // Act
        var result = productPrice.ToString();

        // Assert
        Assert.Equal("100,00", result);
    }

    [Fact]
    public void Equality_ShouldCompareByValue()
    {
        // Arrange
        var price = 99.99m;
        var productPrice1 = new ProductPrice(price);
        var productPrice2 = new ProductPrice(price);

        // Act & Assert
        Assert.Equal(productPrice1, productPrice2);
    }

    [Fact]
    public void Inequality_ShouldNotBeEqualWhenValuesAreDifferent()
    {
        // Arrange
        var productPrice1 = new ProductPrice(99.99m);
        var productPrice2 = new ProductPrice(100.00m);

        // Act & Assert
        Assert.NotEqual(productPrice1, productPrice2);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(1.00)]
    [InlineData(99.99)]
    [InlineData(1000.00)]
    [InlineData(10000.50)]
    public void Constructor_WithValidPrices_ShouldSucceed(decimal price)
    {
        // Act
        var productPrice = new ProductPrice(price);

        // Assert
        Assert.Equal(price, productPrice.Value);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1.00)]
    [InlineData(-99.99)]
    [InlineData(-1000.00)]
    public void Constructor_WithNegativePrices_ShouldThrowDomainException(decimal price)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductPrice(price));
        Assert.Equal("Product price cannot be negative.", exception.Message);
    }
}
