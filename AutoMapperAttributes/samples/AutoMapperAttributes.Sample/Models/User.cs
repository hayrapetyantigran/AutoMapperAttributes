using AutoMapperAttributes;
using AutoMapperAttributes.Sample.DTOs;

namespace AutoMapperAttributes.Sample.Models;

/// <summary>
/// Maps to UserDto (one-way: User → UserDto).
/// Maps to UserSummaryDto with reverse mapping enabled (UserSummaryDto → User also works).
/// </summary>
[MapsTo(typeof(UserDto))]
[MapsTo(typeof(UserSummaryDto), ReverseMap = true)]
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}
