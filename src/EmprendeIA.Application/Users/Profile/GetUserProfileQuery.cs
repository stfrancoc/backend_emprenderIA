using MediatR;

namespace EmprendeIA.Application.Users.Profile;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto?>;

public record UserProfileDto(
    Guid UserId,
    string Name,
    string Email,
    string Role,
    bool Is2FAEnabled,
    string? Bio,
    List<string> Skills,
    List<string> Interests,
    string ExperienceLevel,
    List<string> Industries
);
