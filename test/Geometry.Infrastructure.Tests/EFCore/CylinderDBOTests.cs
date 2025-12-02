using Geometry.Infrastructure.Persistence.EFCore;

namespace Geometry.Infrastructure.Tests;

public class CylinderDBOTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var dbo = new CylinderDBO();

        // Assert
        Assert.NotNull(dbo);
    }

    [Fact]
    public void Id_SetAndGet_ShouldWork()
    {
        // Arrange
        var dbo = new CylinderDBO();
        var id = Guid.NewGuid();

        // Act
        dbo.Id = id;

        // Assert
        Assert.Equal(id, dbo.Id);
    }

    [Fact]
    public void Radius_SetAndGet_ShouldWork()
    {
        // Arrange
        var dbo = new CylinderDBO();
        var radius = 3.5;

        // Act
        dbo.Radius = radius;

        // Assert
        Assert.Equal(radius, dbo.Radius);
    }

    [Fact]
    public void Height_SetAndGet_ShouldWork()
    {
        // Arrange
        var dbo = new CylinderDBO();
        var height = 10.0;

        // Act
        dbo.Height = height;

        // Assert
        Assert.Equal(height, dbo.Height);
    }
}
