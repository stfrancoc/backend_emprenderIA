using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Projects.GenerateBmc;

public class GenerateBmcCommandHandler : IRequestHandler<GenerateBmcCommand, object>
{
    private readonly IAIService _aiService;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public GenerateBmcCommandHandler(
        IAIService aiService, 
        IProjectRepository projectRepository,
        IUserRepository userRepository)
    {
        _aiService = aiService;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<object> Handle(GenerateBmcCommand request, CancellationToken cancellationToken)
{
    var project = await _projectRepository.GetByIdAsync(request.ProjectId);
    if (project == null || project.OwnerId != request.UserId) 
        throw new Exception("Proyecto no encontrado");

    var user = await _userRepository.GetByIdAsync(request.UserId);
    var profile = user?.EntrepreneurProfile;

    var aiInput = new
    {
        entrepreneur = new
        {
            bio = "Emprendedor enfocado en innovación técnica",
            skills = new[] { profile?.Sector ?? "General" },
            industries = new[] { profile?.Sector ?? "Tech" }, 
            experience = $"{profile?.ExperienceYears ?? 0} años"
        },
        project = new
        {
            title = project.Title,
            description = project.Description,
            stage = "Idea" 
        }
    };

    return await _aiService.GenerateBmcAsync(aiInput);
}
}