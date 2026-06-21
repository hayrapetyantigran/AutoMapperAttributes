using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoMapperAttributes;

/// <summary>
/// Extension methods for registering attribute-based AutoMapper mappings with
/// <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers AutoMapper with an <see cref="AttributeMappingProfile"/> that scans the specified
    /// assemblies for classes decorated with <see cref="MapsToAttribute"/>.
    /// Repeated calls merge their assemblies into a single <see cref="IMapper"/> registration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies to scan. Defaults to the calling assembly if none provided.</param>
    [MethodImpl(MethodImplOptions.NoInlining)] // GetCallingAssembly is wrong if this method gets inlined
    public static IServiceCollection AddAutoMapperAttributes(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        var assembliesToScan = assemblies.Length > 0
            ? assemblies
            : [Assembly.GetCallingAssembly()];

        return AddAutoMapperAttributesCore(services, assembliesToScan, configure: null);
    }

    /// <summary>
    /// Registers AutoMapper with an <see cref="AttributeMappingProfile"/> that scans the specified
    /// assemblies, with additional options.
    /// Repeated calls merge their assemblies into a single <see cref="IMapper"/> registration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Configures <see cref="AutoMapperAttributesOptions"/>.</param>
    /// <param name="assemblies">Assemblies to scan. Defaults to the calling assembly if none provided.</param>
    [MethodImpl(MethodImplOptions.NoInlining)] // GetCallingAssembly is wrong if this method gets inlined
    public static IServiceCollection AddAutoMapperAttributes(
        this IServiceCollection services,
        Action<AutoMapperAttributesOptions> configure,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);
        ArgumentNullException.ThrowIfNull(assemblies);

        var assembliesToScan = assemblies.Length > 0
            ? assemblies
            : [Assembly.GetCallingAssembly()];

        return AddAutoMapperAttributesCore(services, assembliesToScan, configure);
    }

    /// <summary>
    /// Registers AutoMapper with an <see cref="AttributeMappingProfile"/> that scans assemblies
    /// containing the specified marker types.
    /// Repeated calls merge their assemblies into a single <see cref="IMapper"/> registration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="markerTypes">Types whose containing assemblies will be scanned.</param>
    public static IServiceCollection AddAutoMapperAttributes(
        this IServiceCollection services,
        IEnumerable<Type> markerTypes)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(markerTypes);

        var assemblies = markerTypes
            .Select(t => t.Assembly)
            .Distinct()
            .ToArray();

        return AddAutoMapperAttributesCore(services, assemblies, configure: null);
    }

    private static IServiceCollection AddAutoMapperAttributesCore(
        IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        Action<AutoMapperAttributesOptions>? configure)
    {
        // A single mutable registration is shared across calls so that assemblies
        // registered by independent modules all end up in the same IMapper.
        var registration = (MappingRegistration?)services
            .FirstOrDefault(d => d.ServiceType == typeof(MappingRegistration))
            ?.ImplementationInstance;

        if (registration is null)
        {
            registration = new MappingRegistration();
            services.AddSingleton(registration);
        }

        foreach (var assembly in assemblies)
        {
            registration.AddAssembly(assembly);
        }

        configure?.Invoke(registration.Options);

        services.TryAddSingleton<IMapper>(sp =>
        {
            var reg = sp.GetRequiredService<MappingRegistration>();
            var loggerFactory = sp.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

            var config = new MapperConfiguration(
                cfg => cfg.AddProfile(new AttributeMappingProfile(reg.Assemblies)),
                loggerFactory);

            if (reg.Options.ValidateConfiguration)
            {
                config.AssertConfigurationIsValid();
            }

            return config.CreateMapper();
        });

        return services;
    }
}
