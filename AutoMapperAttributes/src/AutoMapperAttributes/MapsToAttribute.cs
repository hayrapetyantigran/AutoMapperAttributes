namespace AutoMapperAttributes;

/// <summary>
/// Marks a class for automatic AutoMapper mapping configuration.
/// Apply this attribute to a source model to map it to a destination type.
/// Use <see cref="ReverseMap"/> to also register the inverse mapping.
/// </summary>
/// <example>
/// [MapsTo(typeof(UserDto))]
/// [MapsTo(typeof(UserSummaryDto), ReverseMap = true)]
/// public class User { ... }
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class MapsToAttribute : Attribute
{
    /// <summary>
    /// The destination type to map this class to.
    /// </summary>
    public Type Destination { get; }

    /// <summary>
    /// When true, also registers the reverse mapping (Destination → Source).
    /// </summary>
    public bool ReverseMap { get; init; }

    public MapsToAttribute(Type destination)
    {
        ArgumentNullException.ThrowIfNull(destination);
        Destination = destination;
    }
}
