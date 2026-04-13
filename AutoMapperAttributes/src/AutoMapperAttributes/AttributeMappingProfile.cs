using AutoMapper;
using System.Reflection;

namespace AutoMapperAttributes;

/// <summary>
/// An AutoMapper <see cref="Profile"/> that scans the provided assemblies for classes
/// decorated with <see cref="MapsToAttribute"/> and registers their mappings automatically.
/// </summary>
public sealed class AttributeMappingProfile : Profile
{
    public AttributeMappingProfile(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies)
        {
            RegisterMappingsFromAssembly(assembly);
        }
    }

    public AttributeMappingProfile(params Assembly[] assemblies)
        : this((IEnumerable<Assembly>)assemblies) { }

    private void RegisterMappingsFromAssembly(Assembly assembly)
    {
        var typesWithAttribute = assembly
            .GetTypes()
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
}
