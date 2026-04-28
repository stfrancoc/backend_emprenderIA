using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Application.Users.Profile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _profileRepository;

    public UpdateUserProfileCommandHandler(IUserRepository userRepository, IUserProfileRepository profileRepository)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
    }

    public async Task<bool> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) return false;

        // Update User.Name if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            user.UpdateProfile(request.Name);
            await _userRepository.UpdateAsync(user);
        }

        // Get or create UserProfile
        var profile = await _profileRepository.GetByUserIdAsync(request.UserId);

        if (profile == null)
        {
            profile = new UserProfile(request.UserId);
            profile.Update(request.Bio, request.Skills, request.Interests,
                request.ExperienceLevel, request.Industries);
            await _profileRepository.AddAsync(profile);
        }
        else
        {
            profile.Update(request.Bio, request.Skills, request.Interests,
                request.ExperienceLevel, request.Industries);
            await _profileRepository.UpdateAsync(profile);
        }

        return true;
    }
}
