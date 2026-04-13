using AutoMapper;
using AutoMapperAttributes;
using AutoMapperAttributes.Sample.DTOs;
using AutoMapperAttributes.Sample.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

// ---------------------------------------------------------------------------
// Setup: register AutoMapper using the [MapsTo] attribute scanner.
// All classes in this assembly decorated with [MapsTo] are picked up automatically.
// ---------------------------------------------------------------------------
var services = new ServiceCollection();
services.AddAutoMapperAttributes(Assembly.GetExecutingAssembly());

var provider = services.BuildServiceProvider();
var mapper = provider.GetRequiredService<IMapper>();

// ---------------------------------------------------------------------------
// Demo 1: User → UserDto  (one-way mapping)
// ---------------------------------------------------------------------------
Console.WriteLine("=== Demo 1: User → UserDto (one-way) ===");

var user = new User
{
    Id = 1,
    FirstName = "Alice",
    LastName = "Smith",
    Email = "alice@example.com",
    DateOfBirth = new DateTime(1990, 6, 15)
};

var userDto = mapper.Map<UserDto>(user);
Console.WriteLine($"Mapped UserDto: {userDto.FirstName} {userDto.LastName} | {userDto.Email}");

// ---------------------------------------------------------------------------
// Demo 2: User ↔ UserSummaryDto  (bidirectional via ReverseMap = true)
// ---------------------------------------------------------------------------
Console.WriteLine("\n=== Demo 2: User ↔ UserSummaryDto (bidirectional) ===");

var userSummary = mapper.Map<UserSummaryDto>(user);
Console.WriteLine($"User → UserSummaryDto: Id={userSummary.Id}, Name={userSummary.FirstName} {userSummary.LastName}");

var userFromSummary = mapper.Map<User>(userSummary);
Console.WriteLine($"UserSummaryDto → User: Id={userFromSummary.Id}, Name={userFromSummary.FirstName} {userFromSummary.LastName}");

// ---------------------------------------------------------------------------
// Demo 3: Product ↔ ProductDto  (bidirectional via ReverseMap = true)
// ---------------------------------------------------------------------------
Console.WriteLine("\n=== Demo 3: Product ↔ ProductDto (bidirectional) ===");

var product = new Product
{
    Id = 42,
    Name = "Laptop",
    Price = 999.99m,
    StockQuantity = 15
};

var productDto = mapper.Map<ProductDto>(product);
Console.WriteLine($"Product → ProductDto: {productDto.Name} | ${productDto.Price} | Stock: {productDto.StockQuantity}");

var productFromDto = mapper.Map<Product>(productDto);
Console.WriteLine($"ProductDto → Product: Id={productFromDto.Id}, Name={productFromDto.Name}");

// ---------------------------------------------------------------------------
// Demo 4: Order ↔ OrderDto  (class ↔ record, bidirectional)
// ---------------------------------------------------------------------------
Console.WriteLine("\n=== Demo 4: Order ↔ OrderDto (class ↔ record, bidirectional) ===");

var order = new Order
{
    Id = 7,
    CustomerName = "Bob Johnson",
    TotalAmount = 249.50m,
    OrderDate = new DateTime(2026, 4, 10)
};

var orderDto = mapper.Map<OrderDto>(order);
Console.WriteLine($"Order → OrderDto (record): Id={orderDto.Id}, Customer={orderDto.CustomerName}, Total=${orderDto.TotalAmount}");

var orderFromDto = mapper.Map<Order>(orderDto);
Console.WriteLine($"OrderDto (record) → Order: Id={orderFromDto.Id}, Customer={orderFromDto.CustomerName}, Total=${orderFromDto.TotalAmount}");
