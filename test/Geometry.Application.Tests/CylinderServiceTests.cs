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
    public void Constructor_WithNullRepository_ShouldCreateInstance()
    {
        // Arrange
        ICylinderRepository repository = null!;

        // Act
        var service = new CylinderService(repository);

        // Assert
        Assert.NotNull(service);
        // Note: Methods will throw NullReferenceException if repository is null
    }

    [Fact]
    public async Task Insert_WithNullRepository_ShouldThrowNullReferenceException()
    {
        // Arrange
        ICylinderRepository repository = null!;
        var service = new CylinderService(repository);
        var cylinder = new Cylinder(Guid.NewGuid(), 3.5, 10.0);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.Insert(cylinder));
    }

    [Fact]
    public async Task ReadById_WithNullRepository_ShouldThrowNullReferenceException()
    {
        // Arrange
        ICylinderRepository repository = null!;
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.ReadById(id));
    }

    [Fact]
    public void Constructor_WithValidRepository_ShouldCreateInstance()
    {
        // Arrange
        var repository = new MockCylinderRepository();

        // Act
        var service = new CylinderService(repository);

        // Assert
        Assert.NotNull(service);
    }

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

    [Fact]
    public async Task Insert_ThenReadById_ShouldReturnInsertedCylinder()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 4.5, 12.0);

        // Act
        await service.Insert(cylinder);
        var result = await service.ReadById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(4.5, result.Radius);
        Assert.Equal(12.0, result.Height);
    }

    [Fact]
    public async Task MultipleInserts_ShouldDelegateToRepositoryEachTime()
    {
        // Arrange
        var repository = new MockCylinderRepository();
        var service = new CylinderService(repository);
        var cylinder1 = new Cylinder(Guid.NewGuid(), 2.0, 5.0);
        var cylinder2 = new Cylinder(Guid.NewGuid(), 3.5, 10.0);
        var cylinder3 = new Cylinder(Guid.NewGuid(), 5.0, 15.0);

        // Act
        await service.Insert(cylinder1);
        await service.Insert(cylinder2);
        await service.Insert(cylinder3);

        // Assert
        Assert.Equal(3, repository.InsertCallCount);
    }
}
