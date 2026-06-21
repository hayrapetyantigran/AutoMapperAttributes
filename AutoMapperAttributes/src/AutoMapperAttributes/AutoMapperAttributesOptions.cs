namespace AutoMapperAttributes;

/// <summary>
/// Options controlling how attribute-based mappings are registered.
/// </summary>
public sealed class AutoMapperAttributesOptions
{
    /// <summary>
    /// When true (the default), <c>AssertConfigurationIsValid()</c> is called when the
    /// <see cref="AutoMapper.IMapper"/> is created, so misconfigured maps fail immediately.
    /// Set to false if some destination members are intentionally unmapped.
    /// </summary>
    public bool ValidateConfiguration { get; set; } = true;
}
