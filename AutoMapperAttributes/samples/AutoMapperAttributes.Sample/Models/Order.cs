using AutoMapperAttributes;
using AutoMapperAttributes.Sample.DTOs;

namespace AutoMapperAttributes.Sample.Models;

/// <summary>
/// Maps to an OrderDto record with reverse mapping enabled.
/// AutoMapper matches property names to the record's primary constructor parameters.
/// </summary>
[MapsTo(typeof(OrderDto), ReverseMap = true)]
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
