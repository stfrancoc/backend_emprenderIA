using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;
using System.Text.Json;

namespace EmprendeIA.Application.Projects.GenerateBmc;

public class GenerateBmcCommandHandler : IRequestHandler<GenerateBmcCommand, object>
{
    private readonly IAIService _aiService;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectBmcRepository _bmcRepository;

    public GenerateBmcCommandHandler(
        IAIService aiService, 
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IProjectBmcRepository bmcRepository)
    {
        _aiService = aiService;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _bmcRepository = bmcRepository;
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
                bio = user?.UserProfile?.Bio ?? "Emprendedor enfocado en innovación técnica",
                skills = user?.UserProfile?.Skills ?? new List<string> { profile?.Sector ?? "General" },
                industries = user?.UserProfile?.Industries ?? new List<string> { profile?.Sector ?? "Tech" }, 
                experience = user?.UserProfile?.ExperienceLevel ?? $"{profile?.ExperienceYears ?? 0} años"
            },
            project = new
            {
                title = project.Title,
                description = project.Description,
                stage = project.Stage.ToString()
            }
        };

        var bmcResult = await _aiService.GenerateBmcAsync(aiInput);

        // Persist BMC to database
        try 
        {
            // Assuming bmcResult is an object that can be serialized/deserialized to a known structure
            // In a real scenario, we'd have a specific DTO from AIService
            var json = JsonSerializer.Serialize(bmcResult);
            var bmcData = JsonSerializer.Deserialize<BmcResponseDto>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (bmcData != null)
            {
                var existingBmc = await _bmcRepository.GetByProjectIdAsync(project.Id);
                if (existingBmc == null)
                {
                    existingBmc = new ProjectBmc(project.Id);
                    MapDtoToEntity(bmcData, existingBmc);
                    await _bmcRepository.AddAsync(existingBmc);
                }
                else
                {
                    MapDtoToEntity(bmcData, existingBmc);
                    await _bmcRepository.UpdateAsync(existingBmc);
                }
            }
        }
        catch (Exception ex)
        {
            // Log error but return the result anyway to not break user experience
            Console.WriteLine($"Error persisting BMC: {ex.Message}");
        }

        return bmcResult;
    }

    private void MapDtoToEntity(BmcResponseDto dto, ProjectBmc entity)
    {
        entity.UpdateCanvas(
            dto.CustomerSegments ?? "",
            dto.ValueProposition ?? "",
            dto.Channels ?? "",
            dto.CustomerRelationships ?? "",
            dto.RevenueStreams ?? "",
            dto.KeyResources ?? "",
            dto.KeyActivities ?? "",
            dto.KeyPartners ?? "",
            dto.CostStructure ?? ""
        );
    }

    private class BmcResponseDto
    {
        public string? CustomerSegments { get; set; }
        public string? ValueProposition { get; set; }
        public string? Channels { get; set; }
        public string? CustomerRelationships { get; set; }
        public string? RevenueStreams { get; set; }
        public string? KeyResources { get; set; }
        public string? KeyActivities { get; set; }
        public string? KeyPartners { get; set; }
        public string? CostStructure { get; set; }
    }
}