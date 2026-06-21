namespace AutoMapperAttributes;

/// <summary>
/// Marks a class or struct for automatic AutoMapper mapping configuration.
/// Apply this attribute to a source model to map it to a destination type.
/// Use <see cref="ReverseMap"/> to also register the inverse mapping.
/// </summary>
/// <example>
/// [MapsTo(typeof(UserDto))]
/// [MapsTo(typeof(UserSummaryDto), ReverseMap = true)]
/// public class User { ... }
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
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

    /// <summary>
    /// Declares a mapping from the decorated type to <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">The destination type to map to.</param>
    public MapsToAttribute(Type destination)
    {
        ArgumentNullException.ThrowIfNull(destination);
        Destination = destination;
    }
}
