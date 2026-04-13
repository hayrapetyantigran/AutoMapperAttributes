namespace AutoMapperAttributes.Sample.DTOs;

public record OrderDto(int Id, string CustomerName, decimal TotalAmount, DateTime OrderDate);
