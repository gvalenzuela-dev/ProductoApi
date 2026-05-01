using ProductoApi.Application.Common.Responses;

namespace ProductoApi.Tests.Unit.Application;

public class ResultTests
{
    [Fact]
    public void Ok_WithData_ShouldCreateSuccessResult()
    {
        // Arrange
        var data = "Test Data";

        // Act
        var result = Result<string>.Ok(data);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(data, result.Data);
        Assert.Null(result.Errors);
    }

    [Fact]
    public void Ok_WithNullData_ShouldCreateSuccessResultWithDefault()
    {
        // Arrange & Act
        var result = Result<string>.Ok(null!);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Ok_WithObject_ShouldContainObject()
    {
        // Arrange
        var obj = new { Id = 1, Name = "Test" };

        // Act
        var result = Result<object>.Ok(obj);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(obj, result.Data);
    }

    [Fact]
    public void Ok_WithInt_ShouldContainInt()
    {
        // Arrange
        var value = 42;

        // Act
        var result = Result<int>.Ok(value);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(value, result.Data);
    }

    [Fact]
    public void Fail_WithSingleError_ShouldCreateFailResult()
    {
        // Arrange
        var error = "This is an error";

        // Act
        var result = Result<string>.Fail(error);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.NotNull(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(error, result.Errors[0]);
    }

    [Fact]
    public void Fail_WithEmptyError_ShouldCreateFailResult()
    {
        // Arrange
        var error = string.Empty;

        // Act
        var result = Result<string>.Fail(error);

        // Assert
        Assert.False(result.Success);
        Assert.Single(result.Errors);
        Assert.Equal(error, result.Errors[0]);
    }

    [Fact]
    public void Fail_WithMultipleErrors_ShouldContainAllErrors()
    {
        // Arrange
        var errors = new List<string> { "Error 1", "Error 2", "Error 3" };

        // Act
        var result = Result<string>.Fail(errors);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Equal(3, result.Errors.Count);
        Assert.Equal(errors, result.Errors);
    }

    [Fact]
    public void Fail_WithEmptyErrorList_ShouldCreateFailResult()
    {
        // Arrange
        var errors = new List<string>();

        // Act
        var result = Result<string>.Fail(errors);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Empty(result.Errors);
    }    

    [Fact]
    public void Result_WithDifferentTypes_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var stringResult = Result<string>.Ok("test");
        var intResult = Result<int>.Ok(123);
        var guidResult = Result<Guid>.Ok(Guid.NewGuid());

        // Assert
        Assert.True(stringResult.Success);
        Assert.True(intResult.Success);
        Assert.True(guidResult.Success);
    }

    [Fact]
    public void Result_WithCollectionType_ShouldWork()
    {
        // Arrange
        var list = new List<string> { "item1", "item2" };

        // Act
        var result = Result<List<string>>.Ok(list);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(list, result.Data);
    }

    [Theory]
    [InlineData("Error message 1")]
    [InlineData("Error message 2")]
    [InlineData("Product validation failed")]
    public void Fail_WithVariousMessages_ShouldWork(string message)
    {
        // Act
        var result = Result<bool>.Fail(message);

        // Assert
        Assert.False(result.Success);
        Assert.Contains(message, result.Errors);
    }

    [Fact]
    public void Ok_ShouldNotContainErrors()
    {
        // Act
        var result = Result<string>.Ok("data");

        // Assert
        Assert.Null(result.Errors);
    }

    [Fact]
    public void Fail_ShouldNotContainData()
    {
        // Act
        var result = Result<string>.Fail("error");

        // Assert
        Assert.Null(result.Data);
    }
}
