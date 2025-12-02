using Geometry.Domain.CylinderModel;
using Geometry.Infrastructure.Persistence.EFCore;

namespace Geometry.Infrastructure.Tests;

public class CylinderMapperTests
{
    [Fact]
    public void ToDBO_WithValidCylinder_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 3.5;
        var height = 10.0;
        var cylinder = new Cylinder(id, radius, height);

        // Act
        var cylinderDBO = CylinderMapper.ToDBO(cylinder);

        // Assert
        Assert.NotNull(cylinderDBO);
        Assert.Equal(id, cylinderDBO.Id);
        Assert.Equal(radius, cylinderDBO.Radius);
        Assert.Equal(height, cylinderDBO.Height);
    }

    [Fact]
    public void ToDBO_WithNullCylinder_ShouldThrowArgumentNullException()
    {
        // Arrange
        Cylinder cylinder = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => CylinderMapper.ToDBO(cylinder));
    }

    [Fact]
    public void ToDomain_WithValidCylinderDBO_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 5.0;
        var height = 15.0;
        var cylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = radius,
            Height = height
        };

        // Act
        var cylinder = CylinderMapper.ToDomain(cylinderDBO);

        // Assert
        Assert.NotNull(cylinder);
        Assert.Equal(id, cylinder.Id);
        Assert.Equal(radius, cylinder.Radius);
        Assert.Equal(height, cylinder.Height);
    }

    [Fact]
    public void ToDomain_WithNullCylinderDBO_ShouldThrowArgumentNullException()
    {
        // Arrange
        CylinderDBO cylinderDBO = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => CylinderMapper.ToDomain(cylinderDBO));
    }

    [Fact]
    public void RoundTrip_DomainToDBOToDomain_ShouldPreserveData()
    {
        // Arrange
        var id = Guid.NewGuid();
        var originalCylinder = new Cylinder(id, 4.5, 12.0);

        // Act
        var dbo = CylinderMapper.ToDBO(originalCylinder);
        var resultCylinder = CylinderMapper.ToDomain(dbo);

        // Assert
        Assert.Equal(originalCylinder.Id, resultCylinder.Id);
        Assert.Equal(originalCylinder.Radius, resultCylinder.Radius);
        Assert.Equal(originalCylinder.Height, resultCylinder.Height);
    }

    [Fact]
    public void ToDomain_ThenToDBO_ShouldRoundTripCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var radius = 6.0;
        var height = 18.0;
        var originalCylinderDBO = new CylinderDBO
        {
            Id = id,
            Radius = radius,
            Height = height
        };

        // Act
        var cylinder = CylinderMapper.ToDomain(originalCylinderDBO);
        var roundTrippedCylinderDBO = CylinderMapper.ToDBO(cylinder);

        // Assert
        Assert.Equal(originalCylinderDBO.Id, roundTrippedCylinderDBO.Id);
        Assert.Equal(originalCylinderDBO.Radius, roundTrippedCylinderDBO.Radius);
        Assert.Equal(originalCylinderDBO.Height, roundTrippedCylinderDBO.Height);
    }

    [Fact]
    public void ToDBO_WithDifferentRadiusAndHeight_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var testValues = new[] 
        { 
            (radius: 1.0, height: 2.0),
            (radius: 2.5, height: 5.0),
            (radius: 5.0, height: 10.0),
            (radius: 10.0, height: 20.0)
        };

        foreach (var (radius, height) in testValues)
        {
            var cylinder = new Cylinder(id, radius, height);

            // Act
            var cylinderDBO = CylinderMapper.ToDBO(cylinder);

            // Assert
            Assert.Equal(radius, cylinderDBO.Radius);
            Assert.Equal(height, cylinderDBO.Height);
        }
    }
}
