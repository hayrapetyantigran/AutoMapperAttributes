using AutoMapper;
using System.Reflection;

namespace AutoMapperAttributes;

/// <summary>
/// An AutoMapper <see cref="Profile"/> that scans the provided assemblies for types
/// decorated with <see cref="MapsToAttribute"/> and registers their mappings automatically.
/// </summary>
public sealed class AttributeMappingProfile : Profile
{
    /// <summary>
    /// Creates the profile and registers mappings from all types decorated with
    /// <see cref="MapsToAttribute"/> in the given assemblies.
    /// </summary>
    /// <param name="assemblies">Assemblies to scan for <see cref="MapsToAttribute"/>.</param>
    public AttributeMappingProfile(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies.Distinct())
        {
            RegisterMappingsFromAssembly(assembly);
        }
    }

    /// <inheritdoc cref="AttributeMappingProfile(IEnumerable{Assembly})"/>
    public AttributeMappingProfile(params Assembly[] assemblies)
        : this((IEnumerable<Assembly>)assemblies) { }

    private void RegisterMappingsFromAssembly(Assembly assembly)
    {
        var typesWithAttribute = GetLoadableTypes(assembly)
            .Where(t => t.IsDefined(typeof(MapsToAttribute), inherit: false));

        foreach (var sourceType in typesWithAttribute)
        {
            var attributes = sourceType.GetCustomAttributes<MapsToAttribute>(inherit: false);

            foreach (var attribute in attributes)
            {
                var map = CreateMap(sourceType, attribute.Destination);

                if (attribute.ReverseMap)
                {
                    map.ReverseMap();
                }
            }
        }
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            // Some types may fail to load (e.g. missing optional dependencies);
            // scan the ones that did load instead of failing the whole registration.
            return ex.Types.Where(t => t is not null)!;
        }
    }
}
