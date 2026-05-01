using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductoApi.Application.Features.Products;
using ProductoApi.Application.Features.Products.Requests;
using ProductoApi.Application.Features.Products.Responses;
using ProductoApi.Domain.Products;
using ProductoApi.Domain.Products.ValueObjects;
using ProductoApi.Infrastructure.Persistance;

namespace ProductoApi.Tests.Unit.Application;

public class ProductServiceTests
{
    private readonly Mock<IValidator<CreateProductRequest>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateProductRequest>> _mockUpdateValidator;

    public ProductServiceTests()
    {
        _mockCreateValidator = new Mock<IValidator<CreateProductRequest>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateProductRequest>>();
    }

    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldReturnSuccessWithProductId()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var request = new CreateProductRequest("Test Product", "Test Description", 99.99m);
        var validationResult = new ValidationResult();

        _mockCreateValidator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.NotEqual(Guid.Empty, result.Data);
        Assert.Null(result.Errors);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidRequest_ShouldReturnFailure()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var request = new CreateProductRequest("", "Test Description", 99.99m);
        var validationFailure = new ValidationFailure("Name", "Name is required");
        var validationResult = new ValidationResult(new[] { validationFailure });

        _mockCreateValidator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(Guid.Empty, result.Data);
        Assert.NotNull(result.Errors);
        Assert.Contains("Name is required", result.Errors);
    }

    [Fact]
    public async Task CreateAsync_WithMultipleValidationErrors_ShouldReturnAllErrors()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var request = new CreateProductRequest("", "", -10m);
        var failures = new[]
        {
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Description", "Description is required"),
            new ValidationFailure("Price", "Price must be greater than 0")
        };
        var validationResult = new ValidationResult(failures);

        _mockCreateValidator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(3, result.Errors?.Count);
    }

    [Fact]
    public async Task CreateAsync_ShouldCallValidatorWithRequest()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var request = new CreateProductRequest("Test Product", "Test Description", 99.99m);
        var validationResult = new ValidationResult();

        _mockCreateValidator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        await service.CreateAsync(request);

        // Assert
        _mockCreateValidator.Verify(
            v => v.ValidateAsync(request, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var productId = Guid.NewGuid();
        var productDomainId = new ProductId(productId);
        var product = new Product(
            productDomainId,
            new ProductName("Test Product"),
            new ProductDescription("Test Description"),
            new ProductPrice(99.99m)
        );

        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetByIdAsync(productId);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(productId, result.Data.Id);
        Assert.Equal("Test Product", result.Data.Name);
        Assert.Equal("Test Description", result.Data.Description);
        Assert.Equal(99.99m, result.Data.Price);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var productId = Guid.NewGuid();

        // Act
        var result = await service.GetByIdAsync(productId);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.NotNull(result.Errors);
        Assert.Contains("Product not found", result.Errors);
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_WithProducts_ShouldReturnAllProducts()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var products = new List<Product>
        {
            new(
                new ProductId(Guid.NewGuid()),
                new ProductName("Product 1"),
                new ProductDescription("Description 1"),
                new ProductPrice(10.00m)
            ),
            new(
                new ProductId(Guid.NewGuid()),
                new ProductName("Product 2"),
                new ProductDescription("Description 2"),
                new ProductPrice(20.00m)
            )
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithNoProducts_ShouldReturnEmptyList()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteProduct()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var productId = Guid.NewGuid();
        var productDomainId = new ProductId(productId);
        var product = new Product(
            productDomainId,
            new ProductName("Test Product"),
            new ProductDescription("Test Description"),
            new ProductPrice(99.99m)
        );

        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteAsync(productId);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Data);
        
        // Verify the product was actually deleted
        var deletedProduct = await context.Products.FindAsync(new ProductId(productId));
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var productId = Guid.NewGuid();

        // Act
        var result = await service.DeleteAsync(productId);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Contains("Product not found", result.Errors);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ShouldUpdateProduct()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var productId = Guid.NewGuid();
        var productDomainId = new ProductId(productId);
        var product = new Product(
            productDomainId,
            new ProductName("Old Name"),
            new ProductDescription("Old Description"),
            new ProductPrice(10.00m)
        );

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var request = new UpdateProductRequest(productId, "New Name", "New Description", 20.00m);
        var validationResult = new ValidationResult();

        _mockUpdateValidator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await service.UpdateAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Data);

        // Verify the product was actually updated
        var updatedProduct = await context.Products.FindAsync(new ProductId(productId));
        Assert.NotNull(updatedProduct);
        Assert.Equal("New Name", updatedProduct.Name.Value);
        Assert.Equal("New Description", updatedProduct.Description.Value);
        Assert.Equal(20.00m, updatedProduct.Price.Value);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidRequest_ShouldReturnFailure()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var productId = Guid.NewGuid();
        var request = new UpdateProductRequest(Guid.Empty, "", "", -10m);
        var validationFailure = new ValidationFailure("Id", "Id is required");
        var validationResult = new ValidationResult(new[] { validationFailure });

        _mockUpdateValidator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await service.UpdateAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentProduct_ShouldReturnNotFound()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var service = new ProductService(context, _mockCreateValidator.Object, _mockUpdateValidator.Object);
        var productId = Guid.NewGuid();
        var request = new UpdateProductRequest(productId, "New Name", "New Description", 20.00m);
        var validationResult = new ValidationResult();

        _mockUpdateValidator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await service.UpdateAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Contains("Product not found", result.Errors);
    }

    #endregion
}

