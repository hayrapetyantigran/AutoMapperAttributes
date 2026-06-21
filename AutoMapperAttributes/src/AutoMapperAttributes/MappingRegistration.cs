using System.Reflection;

namespace AutoMapperAttributes;

/// <summary>
/// Accumulates the assemblies and options from all <c>AddAutoMapperAttributes</c> calls
/// on a service collection, so they produce a single merged <c>IMapper</c>.
/// </summary>
internal sealed class MappingRegistration
{
    private readonly List<Assembly> _assemblies = [];

    public IReadOnlyList<Assembly> Assemblies => _assemblies;

    public AutoMapperAttributesOptions Options { get; } = new();

    public void AddAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        if (!_assemblies.Contains(assembly))
        {
            _assemblies.Add(assembly);
        }
    }
}
