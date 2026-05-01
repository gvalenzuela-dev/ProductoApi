using ProductoApi.Domain.Exceptions;

namespace ProductoApi.Tests.Unit.Domain;

public class DomainExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldCreateExceptionWithMessage()
    {
        // Arrange
        var message = "This is a domain error";

        // Act
        var exception = new DomainException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_ShouldInheritFromException()
    {
        // Arrange
        var message = "Domain error";

        // Act
        var exception = new DomainException(message);

        // Assert
        Assert.IsType<DomainException>(exception);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void Constructor_WithEmptyMessage_ShouldCreateException()
    {
        // Arrange
        var emptyMessage = string.Empty;

        // Act
        var exception = new DomainException(emptyMessage);

        // Assert
        Assert.Equal(emptyMessage, exception.Message);
    }
    

    [Fact]
    public void Constructor_WithLongMessage_ShouldPreserveFullMessage()
    {
        // Arrange
        var longMessage = string.Concat(Enumerable.Repeat("This is a long error message. ", 10));

        // Act
        var exception = new DomainException(longMessage);

        // Assert
        Assert.Equal(longMessage, exception.Message);
    }

    [Fact]
    public async Task Exception_CanBeThrownAndCaught()
    {
        // Arrange
        var message = "Test error message";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
        {
            throw new DomainException(message);
        });

        Assert.Equal(message, exception.Message);
    }    

    [Theory]
    [InlineData("Product name cannot be empty.")]
    [InlineData("Product name cannot exceed 100 characters.")]
    [InlineData("Product description cannot be empty.")]
    [InlineData("Product description cannot exceed 500 characters.")]
    [InlineData("Product price cannot be negative.")]
    public void Constructor_WithCommonDomainMessages_ShouldCreateExceptionProperly(string message)
    {
        // Act
        var exception = new DomainException(message);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.IsType<DomainException>(exception);
    }

    [Fact]
    public void ToString_ShouldIncludeExceptionTypeName()
    {
        // Arrange
        var message = "Domain error occurred";
        var exception = new DomainException(message);

        // Act
        var exceptionString = exception.ToString();

        // Assert
        Assert.Contains("DomainException", exceptionString);
        Assert.Contains(message, exceptionString);
    }
}
