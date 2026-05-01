using FluentValidation.TestHelper;
using ProductoApi.Application.Features.Products.Requests;
using ProductoApi.Application.Features.Products.Validators;

namespace ProductoApi.Tests.Unit.Application;

public class UpdateProductRequestValidatorTests
{
    private readonly UpdateProductRequestValidator _validator = new();

    [Fact]
    public void Validate_WithValidRequest_ShouldPass()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyId_ShouldFail()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.Empty,
            Name: "Valid Product",
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: string.Empty,
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNullName_ShouldFail()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: null!,
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameExceeding100Characters_ShouldFail()
    {
        // Arrange
        var longName = new string('a', 101);
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: longName,
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameOf100Characters_ShouldPass()
    {
        // Arrange
        var name = new string('a', 100);
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: name,
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithEmptyDescription_ShouldFail()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: string.Empty,
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithNullDescription_ShouldFail()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: null!,
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithDescriptionExceeding500Characters_ShouldFail()
    {
        // Arrange
        var longDescription = new string('a', 501);
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: longDescription,
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithDescriptionOf500Characters_ShouldPass()
    {
        // Arrange
        var description = new string('a', 500);
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: description,
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithNegativePrice_ShouldFail()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: "Valid Description",
            Price: -10.00m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_WithZeroPrice_ShouldFail()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: "Valid Description",
            Price: 0m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_WithPositivePrice_ShouldPass()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: "Valid Description",
            Price: 0.01m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("Product 1")]
    [InlineData("A longer product name")]
    public void Validate_WithValidNames_ShouldPass(string name)
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: name,
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(10.00)]
    [InlineData(100.50)]
    [InlineData(9999.99)]
    public void Validate_WithVariousPrices_ShouldPass(decimal price)
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.NewGuid(),
            Name: "Valid Product",
            Description: "Valid Description",
            Price: price
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_WithMultipleErrors_ShouldFailOnAllInvalidFields()
    {
        // Arrange
        var request = new UpdateProductRequest(
            Id: Guid.Empty,
            Name: string.Empty,
            Description: string.Empty,
            Price: -10m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_WithValidGuid_ShouldPass()
    {
        // Arrange
        var validGuid = Guid.NewGuid();
        var request = new UpdateProductRequest(
            Id: validGuid,
            Name: "Valid Product",
            Description: "Valid Description",
            Price: 99.99m
        );

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
