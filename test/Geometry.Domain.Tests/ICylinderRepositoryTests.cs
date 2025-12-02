using Geometry.Domain.CylinderModel;

namespace Geometry.Domain.Tests;

/// <summary>
/// Mock implementation of ICylinderRepository for testing interface contract.
/// </summary>
public class MockCylinderRepositoryForInterfaceTests : ICylinderRepository
{
    public bool ReadByIdCalled { get; private set; }
    public bool InsertCalled { get; private set; }
    public bool UpdateCalled { get; private set; }
    public bool DeleteCalled { get; private set; }

    public Task<Cylinder?> ReadById(Guid id)
    {
        ReadByIdCalled = true;
        return Task.FromResult<Cylinder?>(null);
    }

    public Task<Guid> Insert(Cylinder cylinder)
    {
        InsertCalled = true;
        return Task.FromResult(cylinder?.Id ?? Guid.Empty);
    }

    public Task<bool> Update(Cylinder cylinder)
    {
        UpdateCalled = true;
        return Task.FromResult(false);
    }

    public Task<bool> Delete(Guid id)
    {
        DeleteCalled = true;
        return Task.FromResult(false);
    }
}

public class ICylinderRepositoryTests
{
    [Fact]
    public async Task ReadById_ShouldBeCallable()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var id = Guid.NewGuid();

        // Act
        await repository.ReadById(id);

        // Assert
        Assert.True(repository.ReadByIdCalled);
    }

    [Fact]
    public async Task ReadById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var id = Guid.NewGuid();

        // Act
        var result = await repository.ReadById(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Insert_ShouldBeCallable()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var cylinder = new Cylinder(Guid.NewGuid(), 3.5, 10.0);

        // Act
        await repository.Insert(cylinder);

        // Assert
        Assert.True(repository.InsertCalled);
    }

    [Fact]
    public async Task Insert_ShouldReturnGuid()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 3.5, 10.0);

        // Act
        var result = await repository.Insert(cylinder);

        // Assert
        Assert.Equal(id, result);
    }

    [Fact]
    public async Task Update_ShouldBeCallable()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var cylinder = new Cylinder(Guid.NewGuid(), 3.5, 10.0);

        // Act
        await repository.Update(cylinder);

        // Assert
        Assert.True(repository.UpdateCalled);
    }

    [Fact]
    public async Task Update_ShouldReturnBool()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var cylinder = new Cylinder(Guid.NewGuid(), 3.5, 10.0);

        // Act
        var result = await repository.Update(cylinder);

        // Assert
        Assert.IsType<bool>(result);
    }

    [Fact]
    public async Task Delete_ShouldBeCallable()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var id = Guid.NewGuid();

        // Act
        await repository.Delete(id);

        // Assert
        Assert.True(repository.DeleteCalled);
    }

    [Fact]
    public async Task Delete_ShouldReturnBool()
    {
        // Arrange
        var repository = new MockCylinderRepositoryForInterfaceTests();
        var id = Guid.NewGuid();

        // Act
        var result = await repository.Delete(id);

        // Assert
        Assert.IsType<bool>(result);
    }
}
