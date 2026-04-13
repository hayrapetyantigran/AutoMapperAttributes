# AutoMapperAttributes

A lightweight library that eliminates boilerplate AutoMapper profile registrations by letting you declare mappings directly on your model classes using the `[MapsTo]` attribute.

## The Problem

Every time you add a new model, you must manually update your mapping profile:

```csharp
// ❌ Manual mapping profile maintenance
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Product, ProductDto>().ReverseMap();
        // ... every new model requires a new line here
    }
}
```

## The Solution

Decorate your models with `[MapsTo]` and mappings are registered automatically at startup via reflection:

```csharp
// ✅ Declare mappings where they belong — on the model
[MapsTo(typeof(UserDto))]
[MapsTo(typeof(UserSummaryDto), ReverseMap = true)]
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    // ...
}
```

## Getting Started

### 1. Decorate your models

```csharp
using AutoMapperAttributes;

// One-way: User → UserDto
[MapsTo(typeof(UserDto))]
public class User { ... }

// Bidirectional: Product ↔ ProductDto
[MapsTo(typeof(ProductDto), ReverseMap = true)]
public class Product { ... }

// Multiple targets on a single class
[MapsTo(typeof(UserDto))]
[MapsTo(typeof(UserSummaryDto), ReverseMap = true)]
public class User { ... }

// Mapping to a record — AutoMapper matches property names to constructor parameters
[MapsTo(typeof(OrderDto), ReverseMap = true)]
public class Order { ... }

public record OrderDto(int Id, string CustomerName, decimal TotalAmount, DateTime OrderDate);
```

### 2. Register at startup

**With Microsoft.Extensions.DependencyInjection:**

```csharp
// Scan the calling assembly (default)
services.AddAutoMapperAttributes();

// Or specify assemblies explicitly
services.AddAutoMapperAttributes(typeof(User).Assembly, typeof(SomeOtherModel).Assembly);

// Or use marker types
services.AddAutoMapperAttributes([typeof(User), typeof(SomeOtherModel)]);
```

**Without DI (manual setup):**

```csharp
var config = new MapperConfiguration(cfg =>
    cfg.AddProfile(new AttributeMappingProfile(typeof(User).Assembly)));

var mapper = config.CreateMapper();
```

### 3. Inject and use

```csharp
public class UserService(IMapper mapper)
{
    public UserDto GetUser() =>
        mapper.Map<UserDto>(new User { Id = 1, FirstName = "Alice" });
}
```

## Mapping to Records

Records are fully supported as mapping destinations. AutoMapper matches source property names to the record's primary constructor parameter names (case-insensitive):

```csharp
public record OrderDto(int Id, string CustomerName, decimal TotalAmount, DateTime OrderDate);

[MapsTo(typeof(OrderDto), ReverseMap = true)]
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
```

| Scenario | Works? |
|---|---|
| Class → Record | ✅ |
| Record → Class (ReverseMap) | ✅ |
| Record → Record | ✅ |
| Mismatched names | ❌ needs custom AutoMapper config |

## `[MapsTo]` Attribute Reference

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `destination` | `Type` | *(required)* | The destination type to map to |
| `ReverseMap` | `bool` | `false` | Also register the inverse mapping |

## Project Structure

```
AutoMapperAttributes/
├── src/
│   └── AutoMapperAttributes/          # Class library
│       ├── MapsToAttribute.cs          # The [MapsTo] attribute
│       ├── AttributeMappingProfile.cs  # AutoMapper Profile scanner
│       └── ServiceCollectionExtensions.cs
└── samples/
    └── AutoMapperAttributes.Sample/   # Console app demo
        ├── Models/
        │   ├── User.cs
        │   ├── Product.cs
        │   └── Order.cs
        ├── DTOs/
        │   ├── UserDto.cs
        │   ├── UserSummaryDto.cs
        │   ├── ProductDto.cs
        │   └── OrderDto.cs        # record DTO
        └── Program.cs
```

## Requirements

- .NET 8+
- AutoMapper 14+
