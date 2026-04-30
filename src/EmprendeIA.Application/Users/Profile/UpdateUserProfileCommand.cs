using MediatR;
using System.Text.Json.Serialization;

namespace EmprendeIA.Application.Users.Profile;

public record UpdateUserProfileCommand(
    string? Name,
    string? Bio,
    List<string>? Skills,
    List<string>? Interests,
    string? ExperienceLevel,
    List<string>? Industries
) : IRequest<UserProfileDto?>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}
