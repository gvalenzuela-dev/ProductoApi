using ProductoApi.Domain.Products;
using ProductoApi.Domain.Products.ValueObjects;

namespace ProductoApi.Tests.Unit.Domain;

public class ProductTests
{
    private ProductId CreateProductId() => ProductId.New();

    private ProductName CreateValidProductName(string name = "Valid Product Name") => new(name);

    private ProductDescription CreateValidProductDescription(string description = "Valid Product Description") => new(description);

    private ProductPrice CreateValidProductPrice(decimal price = 99.99m) => new(price);

    [Fact]
    public void Constructor_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var productId = CreateProductId();
        var name = CreateValidProductName();
        var description = CreateValidProductDescription();
        var price = CreateValidProductPrice();

        // Act
        var product = new Product(productId, name, description, price);

        // Assert
        Assert.Equal(productId, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
    }

    [Fact]
    public void Constructor_ShouldSetAllProperties()
    {
        // Arrange
        var productId = new ProductId(Guid.NewGuid());
        var name = new ProductName("Test Product");
        var description = new ProductDescription("Test Description");
        var price = new ProductPrice(50.00m);

        // Act
        var product = new Product(productId, name, description, price);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(productId.Value, product.Id.Value);
        Assert.Equal("Test Product", product.Name.Value);
        Assert.Equal("Test Description", product.Description.Value);
        Assert.Equal(50.00m, product.Price.Value);
    }

    [Fact]
    public void Update_ShouldChangeProductProperties()
    {
        // Arrange
        var product = new Product(
            CreateProductId(),
            CreateValidProductName("Original Name"),
            CreateValidProductDescription("Original Description"),
            CreateValidProductPrice(99.99m)
        );

        var newName = new ProductName("Updated Name");
        var newDescription = new ProductDescription("Updated Description");
        var newPrice = new ProductPrice(149.99m);

        // Act
        product.Update(newName, newDescription, newPrice);

        // Assert
        Assert.Equal(newName, product.Name);
        Assert.Equal(newDescription, product.Description);
        Assert.Equal(newPrice, product.Price);
    }

    [Fact]
    public void Update_ShouldNotChangeProductId()
    {
        // Arrange
        var productId = CreateProductId();
        var product = new Product(
            productId,
            CreateValidProductName(),
            CreateValidProductDescription(),
            CreateValidProductPrice()
        );

        var originalId = product.Id;

        // Act
        product.Update(
            CreateValidProductName("New Name"),
            CreateValidProductDescription("New Description"),
            CreateValidProductPrice(200.00m)
        );

        // Assert
        Assert.Equal(originalId, product.Id);
    }

    [Fact]
    public void Update_WithZeroPrice_ShouldAllowUpdate()
    {
        // Arrange
        var product = new Product(
            CreateProductId(),
            CreateValidProductName(),
            CreateValidProductDescription(),
            CreateValidProductPrice(99.99m)
        );

        var newPrice = new ProductPrice(0m);

        // Act
        product.Update(CreateValidProductName(), CreateValidProductDescription(), newPrice);

        // Assert
        Assert.Equal(0m, product.Price.Value);
    }

    [Fact]
    public void Update_WithMinimumLengthName_ShouldAllowUpdate()
    {
        // Arrange
        var product = new Product(
            CreateProductId(),
            CreateValidProductName("Original"),
            CreateValidProductDescription(),
            CreateValidProductPrice()
        );

        var newName = new ProductName("A");

        // Act
        product.Update(newName, CreateValidProductDescription(), CreateValidProductPrice());

        // Assert
        Assert.Equal("A", product.Name.Value);
    }

    [Fact]
    public void Update_WithMaximumLengthName_ShouldAllowUpdate()
    {
        // Arrange
        var product = new Product(
            CreateProductId(),
            CreateValidProductName(),
            CreateValidProductDescription(),
            CreateValidProductPrice()
        );

        var maxLengthName = new string('a', 100);
        var newName = new ProductName(maxLengthName);

        // Act
        product.Update(newName, CreateValidProductDescription(), CreateValidProductPrice());

        // Assert
        Assert.Equal(maxLengthName, product.Name.Value);
    }

    [Fact]
    public void Update_WithMinimumLengthDescription_ShouldAllowUpdate()
    {
        // Arrange
        var product = new Product(
            CreateProductId(),
            CreateValidProductName(),
            CreateValidProductDescription("Original"),
            CreateValidProductPrice()
        );

        var newDescription = new ProductDescription("A");

        // Act
        product.Update(CreateValidProductName(), newDescription, CreateValidProductPrice());

        // Assert
        Assert.Equal("A", product.Description.Value);
    }

    [Fact]
    public void Update_WithMaximumLengthDescription_ShouldAllowUpdate()
    {
        // Arrange
        var product = new Product(
            CreateProductId(),
            CreateValidProductName(),
            CreateValidProductDescription(),
            CreateValidProductPrice()
        );

        var maxLengthDescription = new string('a', 500);
        var newDescription = new ProductDescription(maxLengthDescription);

        // Act
        product.Update(CreateValidProductName(), newDescription, CreateValidProductPrice());

        // Assert
        Assert.Equal(maxLengthDescription, product.Description.Value);
    }

    [Fact]
    public void Update_MultipleTimesWithDifferentValues_ShouldUpdateCorrectly()
    {
        // Arrange
        var product = new Product(
            CreateProductId(),
            CreateValidProductName("Name 1"),
            CreateValidProductDescription("Description 1"),
            CreateValidProductPrice(10.00m)
        );

        // Act - First Update
        product.Update(
            CreateValidProductName("Name 2"),
            CreateValidProductDescription("Description 2"),
            CreateValidProductPrice(20.00m)
        );

        // Assert First Update
        Assert.Equal("Name 2", product.Name.Value);
        Assert.Equal("Description 2", product.Description.Value);
        Assert.Equal(20.00m, product.Price.Value);

        // Act - Second Update
        product.Update(
            CreateValidProductName("Name 3"),
            CreateValidProductDescription("Description 3"),
            CreateValidProductPrice(30.00m)
        );

        // Assert Second Update
        Assert.Equal("Name 3", product.Name.Value);
        Assert.Equal("Description 3", product.Description.Value);
        Assert.Equal(30.00m, product.Price.Value);
    }

    [Fact]
    public void Constructor_WithUniqueIds_ShouldCreateDifferentProducts()
    {
        // Arrange
        var name = CreateValidProductName("Same Name");
        var description = CreateValidProductDescription("Same Description");
        var price = CreateValidProductPrice(99.99m);

        // Act
        var product1 = new Product(CreateProductId(), name, description, price);
        var product2 = new Product(CreateProductId(), name, description, price);

        // Assert
        Assert.NotEqual(product1.Id, product2.Id);
    }

    [Theory]
    [InlineData(0.00)]
    [InlineData(0.01)]
    [InlineData(99.99)]
    [InlineData(999.99)]
    [InlineData(10000.00)]
    public void Constructor_WithDifferentPrices_ShouldSucceed(decimal price)
    {
        // Act
        var product = new Product(
            CreateProductId(),
            CreateValidProductName(),
            CreateValidProductDescription(),
            new ProductPrice(price)
        );

        // Assert
        Assert.Equal(price, product.Price.Value);
    }
}
