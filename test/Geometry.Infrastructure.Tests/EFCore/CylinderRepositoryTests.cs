using Geometry.Domain.CylinderModel;
using Geometry.Infrastructure.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Geometry.Infrastructure.Tests;

public class CylinderRepositoryTests
{
    private GeometryDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<GeometryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new GeometryDbContext(options);
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange
        GeometryDbContext context = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CylinderRepository(context));
    }

    [Fact]
    public void Constructor_WithValidContext_ShouldCreateInstance()
    {
        // Arrange
        using var context = CreateContext();

        // Act
        var repository = new CylinderRepository(context);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task Insert_WithNewCylinder_ShouldSaveAndReturnId()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 3.5, 10.0);

        // Act
        var result = await repository.Insert(cylinder);

        // Assert
        Assert.Equal(id, result);

        // Verify it was saved
        var retrieved = await repository.ReadById(id);
        Assert.NotNull(retrieved);
        Assert.Equal(3.5, retrieved.Radius);
        Assert.Equal(10.0, retrieved.Height);
    }

    [Fact]
    public async Task Insert_WithNullCylinder_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.Insert(null!));
    }

    [Fact]
    public async Task ReadById_WithExistingCylinder_ShouldReturnCylinder()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 5.0, 15.0);
        await repository.Insert(cylinder);

        // Act
        var result = await repository.ReadById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(5.0, result.Radius);
        Assert.Equal(15.0, result.Height);
    }

    [Fact]
    public async Task ReadById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.ReadById(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_WithExistingCylinder_ShouldUpdateAndReturnTrue()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 2.5, 6.0);
        await repository.Insert(cylinder);

        var updatedCylinder = new Cylinder(id, 5.5, 12.0);

        // Act
        var result = await repository.Update(updatedCylinder);

        // Assert
        Assert.True(result);
        var retrieved = await repository.ReadById(id);
        Assert.NotNull(retrieved);
        Assert.Equal(5.5, retrieved.Radius);
        Assert.Equal(12.0, retrieved.Height);
    }

    [Fact]
    public async Task Update_WithNonExistentCylinder_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var nonExistentId = Guid.NewGuid();
        var cylinder = new Cylinder(nonExistentId, 3.5, 10.0);

        // Act
        var result = await repository.Update(cylinder);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Update_WithNullCylinder_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.Update(null!));
    }

    [Fact]
    public async Task Delete_WithExistingCylinder_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var id = Guid.NewGuid();
        var cylinder = new Cylinder(id, 3.5, 10.0);
        await repository.Insert(cylinder);

        // Act
        var result = await repository.Delete(id);

        // Assert
        Assert.True(result);
        var retrieved = await repository.ReadById(id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new CylinderRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.Delete(nonExistentId);

        // Assert
        Assert.False(result);
    }
}
