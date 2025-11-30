using Geometry.Domain.CylinderModel;

namespace Geometry.Application.Tests;

/// <summary>
/// Mock implementation of ICylinderRepository for testing purposes.
/// </summary>
public class MockCylinderRepository : ICylinderRepository
{
    private readonly Dictionary<Guid, Cylinder> _cylinders = new();
    public int ReadByIdCallCount { get; private set; }
    public int InsertCallCount { get; private set; }
    public int UpdateCallCount { get; private set; }
    public int DeleteCallCount { get; private set; }

    public Task<Cylinder?> ReadById(Guid id)
    {
        ReadByIdCallCount++;
        _cylinders.TryGetValue(id, out var cylinder);
        return Task.FromResult<Cylinder?>(cylinder);
    }

    public Task<Guid> Insert(Cylinder cylinder)
    {
        InsertCallCount++;
        if (cylinder != null)
        {
            _cylinders[cylinder.Id] = cylinder;
            return Task.FromResult(cylinder.Id);
        }
        throw new ArgumentNullException(nameof(cylinder));
    }

    public Task<bool> Update(Cylinder cylinder)
    {
        UpdateCallCount++;
        if (cylinder == null)
        {
            throw new ArgumentNullException(nameof(cylinder));
        }

        if (_cylinders.ContainsKey(cylinder.Id))
        {
            _cylinders[cylinder.Id] = cylinder;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> Delete(Guid id)
    {
        DeleteCallCount++;
        return Task.FromResult(_cylinders.Remove(id));
    }
}

public class CylinderServiceTests
{
    [Fact]
    public async Task Insert_WithValidCylinder_ShouldReturnId()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 3.5, 10.0);

        // Act
        var result = await service.Insert(cylinder);

        // Assert
        Assert.Equal(id, result);
        Assert.Equal(1, repository.InsertCallCount);
    }

    [Fact]
    public async Task ReadById_WithExistingCylinder_ShouldReturnCylinder()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 3.5, 10.0);
        await service.Insert(cylinder);

        // Act
        var result = await service.ReadById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(3.5, result.Radius);
        Assert.Equal(10.0, result.Height);
    }

    [Fact]
    public async Task ReadById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await service.ReadById(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_WithExistingCylinder_ShouldReturnTrue()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 3.5, 10.0);
        await service.Insert(cylinder);

        var updatedCylinder = new Cylinder(id, 5.0, 12.0);

        // Act
        var result = await service.Update(updatedCylinder);

        // Assert
        Assert.True(result);
        Assert.Equal(1, repository.UpdateCallCount);
    }

    [Fact]
    public async Task Update_WithNonExistentCylinder_ShouldReturnFalse()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var nonExistentId = Guid.NewGuid();
        var cylinder = new Cylinder(nonExistentId, 3.5, 10.0);

        // Act
        var result = await service.Update(cylinder);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Delete_WithExistingCylinder_ShouldReturnTrue()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 3.5, 10.0);
        await service.Insert(cylinder);

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result);
        Assert.Equal(1, repository.DeleteCallCount);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await service.Delete(nonExistentId);

        // Assert
        Assert.False(result);
    }
}
