using AutoMapperAttributes;
using AutoMapperAttributes.Sample.DTOs;

namespace AutoMapperAttributes.Sample.Models;

/// <summary>
/// Maps to ProductDto with reverse mapping (ProductDto → Product also works).
/// </summary>
[MapsTo(typeof(ProductDto), ReverseMap = true)]
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
