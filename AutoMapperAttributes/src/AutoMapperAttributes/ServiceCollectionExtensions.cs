using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;

namespace AutoMapperAttributes;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers AutoMapper with an <see cref="AttributeMappingProfile"/> that scans the specified
    /// assemblies for classes decorated with <see cref="MapsToAttribute"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblies">Assemblies to scan. Defaults to the calling assembly if none provided.</param>
    public static IServiceCollection AddAutoMapperAttributes(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var assembliesToScan = assemblies.Length > 0
            ? assemblies
            : [Assembly.GetCallingAssembly()];

        services.AddSingleton<IMapper>(_ =>
        {
            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AttributeMappingProfile(assembliesToScan)), NullLoggerFactory.Instance);

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        });

        return services;
    }

    /// <summary>
    /// Registers AutoMapper with an <see cref="AttributeMappingProfile"/> that scans assemblies
    /// containing the specified marker types.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="markerTypes">Types whose containing assemblies will be scanned.</param>
    public static IServiceCollection AddAutoMapperAttributes(
        this IServiceCollection services,
        IEnumerable<Type> markerTypes)
    {
        var assemblies = markerTypes
            .Select(t => t.Assembly)
            .Distinct()
            .ToArray();

        return services.AddAutoMapperAttributes(assemblies);
    }
}
