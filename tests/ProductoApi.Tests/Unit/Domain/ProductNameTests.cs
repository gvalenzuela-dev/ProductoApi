using ProductoApi.Domain.Exceptions;
using ProductoApi.Domain.Products.ValueObjects;

namespace ProductoApi.Tests.Unit.Domain;

public class ProductNameTests
{
    [Fact]
    public void Constructor_WithValidName_ShouldCreateProductName()
    {
        // Arrange
        var name = "Valid Product Name";

        // Act
        var productName = new ProductName(name);

        // Assert
        Assert.Equal(name, productName.Value);
    }

    [Fact]
    public void Constructor_WithEmptyString_ShouldThrowDomainException()
    {
        // Arrange
        var emptyName = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductName(emptyName));
        Assert.Equal("Product name cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithWhitespaceOnly_ShouldThrowDomainException()
    {
        // Arrange
        var whitespaceName = "   ";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductName(whitespaceName));
        Assert.Equal("Product name cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNull_ShouldThrowDomainException()
    {
        // Arrange
        string? nullName = null;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductName(nullName!));
        Assert.Equal("Product name cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNameExceeding100Characters_ShouldThrowDomainException()
    {
        // Arrange
        var longName = new string('a', 101);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductName(longName));
        Assert.Equal("Product name cannot exceed 100 characters.", exception.Message);
    }

    [Fact]
    public void Constructor_With100Characters_ShouldSucceed()
    {
        // Arrange
        var name = new string('a', 100);

        // Act
        var productName = new ProductName(name);

        // Assert
        Assert.Equal(name, productName.Value);
        Assert.Equal(100, productName.Value.Length);
    }

    [Fact]
    public void Constructor_With99Characters_ShouldSucceed()
    {
        // Arrange
        var name = new string('a', 99);

        // Act
        var productName = new ProductName(name);

        // Assert
        Assert.Equal(name, productName.Value);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var name = "Product Name";
        var productName = new ProductName(name);

        // Act
        var result = productName.ToString();

        // Assert
        Assert.Equal(name, result);
    }

    [Fact]
    public void Equality_ShouldCompareByValue()
    {
        // Arrange
        var name = "Product Name";
        var productName1 = new ProductName(name);
        var productName2 = new ProductName(name);

        // Act & Assert
        Assert.Equal(productName1, productName2);
    }

    [Fact]
    public void Inequality_ShouldNotBeEqualWhenValuesAreDifferent()
    {
        // Arrange
        var productName1 = new ProductName("Product 1");
        var productName2 = new ProductName("Product 2");

        // Act & Assert
        Assert.NotEqual(productName1, productName2);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("Product")]
    [InlineData("A new product with a very descriptive name")]
    [InlineData("123 Special Characters !@#$%")]
    public void Constructor_WithValidNames_ShouldSucceed(string name)
    {
        // Act
        var productName = new ProductName(name);

        // Assert
        Assert.Equal(name, productName.Value);
    }
}
