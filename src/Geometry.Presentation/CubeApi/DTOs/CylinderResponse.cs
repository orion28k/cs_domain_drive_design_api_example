namespace Geometry.Presentation.CubeApi.DTOs;

/// <summary>
/// Data Transfer Object representing a cylinder entity in API responses.
/// Used to return cylinder data from GET /cylinder/{id} endpoint.
/// </summary>
public class CylinderResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the cylinder.
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the radius of the cylinder's base.
    /// </summary>
    /// <example>3.5</example>
    public double Radius { get; set; }

    /// <summary>
    /// Gets or sets the height of the cylinder.
    /// </summary>
    /// <example>10.0</example>
    public double Height { get; set; }
}
