using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Application.Users.Profile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _profileRepository;

    public GetUserProfileQueryHandler(IUserRepository userRepository, IUserProfileRepository profileRepository)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
    }

    public async Task<UserProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) return null;

        var profile = await _profileRepository.GetByUserIdAsync(request.UserId);

        return new UserProfileDto(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.Is2FAEnabled,
            profile?.Bio,
            profile?.Skills ?? new List<string>(),
            profile?.Interests ?? new List<string>(),
            profile?.ExperienceLevel ?? "junior",
            profile?.Industries ?? new List<string>()
        );
    }
}
