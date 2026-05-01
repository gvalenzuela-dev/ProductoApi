using ProductoApi.Domain.Exceptions;
using ProductoApi.Domain.Products.ValueObjects;

namespace ProductoApi.Tests.Unit.Domain;

public class ProductDescriptionTests
{
    [Fact]
    public void Constructor_WithValidDescription_ShouldCreateProductDescription()
    {
        // Arrange
        var description = "Valid Product Description";

        // Act
        var productDescription = new ProductDescription(description);

        // Assert
        Assert.Equal(description, productDescription.Value);
    }

    [Fact]
    public void Constructor_WithEmptyString_ShouldThrowDomainException()
    {
        // Arrange
        var emptyDescription = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductDescription(emptyDescription));
        Assert.Equal("Product description cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithWhitespaceOnly_ShouldThrowDomainException()
    {
        // Arrange
        var whitespaceDescription = "   ";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductDescription(whitespaceDescription));
        Assert.Equal("Product description cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNull_ShouldThrowDomainException()
    {
        // Arrange
        string? nullDescription = null;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductDescription(nullDescription!));
        Assert.Equal("Product description cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithDescriptionExceeding500Characters_ShouldThrowDomainException()
    {
        // Arrange
        var longDescription = new string('a', 501);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new ProductDescription(longDescription));
        Assert.Equal("Product description cannot exceed 500 characters.", exception.Message);
    }

    [Fact]
    public void Constructor_With500Characters_ShouldSucceed()
    {
        // Arrange
        var description = new string('a', 500);

        // Act
        var productDescription = new ProductDescription(description);

        // Assert
        Assert.Equal(description, productDescription.Value);
        Assert.Equal(500, productDescription.Value.Length);
    }

    [Fact]
    public void Constructor_With499Characters_ShouldSucceed()
    {
        // Arrange
        var description = new string('a', 499);

        // Act
        var productDescription = new ProductDescription(description);

        // Assert
        Assert.Equal(description, productDescription.Value);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var description = "Product Description";
        var productDescription = new ProductDescription(description);

        // Act
        var result = productDescription.ToString();

        // Assert
        Assert.Equal(description, result);
    }

    [Fact]
    public void Equality_ShouldCompareByValue()
    {
        // Arrange
        var description = "Product Description";
        var productDescription1 = new ProductDescription(description);
        var productDescription2 = new ProductDescription(description);

        // Act & Assert
        Assert.Equal(productDescription1, productDescription2);
    }

    [Fact]
    public void Inequality_ShouldNotBeEqualWhenValuesAreDifferent()
    {
        // Arrange
        var productDescription1 = new ProductDescription("Description 1");
        var productDescription2 = new ProductDescription("Description 2");

        // Act & Assert
        Assert.NotEqual(productDescription1, productDescription2);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("Short description")]
    [InlineData("A longer description with multiple words and sentences. This product is excellent and provides great value to our customers.")]
    [InlineData("Description with special characters !@#$%^&*()_+-=[]{}|;:',.<>?/")]
    [InlineData("Description with\nmultiple\nlines")]
    public void Constructor_WithValidDescriptions_ShouldSucceed(string description)
    {
        // Act
        var productDescription = new ProductDescription(description);

        // Assert
        Assert.Equal(description, productDescription.Value);
    }
}
