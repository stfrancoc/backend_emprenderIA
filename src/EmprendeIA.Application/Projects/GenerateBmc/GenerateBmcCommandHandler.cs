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

        var aiInput = new BmcGenerationRequest(
            request.UserId,
            request.ProjectId,
            new BmcEntrepreneurContext(
                user?.UserProfile?.Bio ?? "Emprendedor enfocado en innovación técnica",
                user?.UserProfile?.Skills ?? new List<string> { profile?.Sector ?? "General" },
                user?.UserProfile?.Industries ?? new List<string> { profile?.Sector ?? "Tech" },
                profile?.ExperienceYears ?? 0,
                user?.UserProfile?.ExperienceLevel ?? string.Empty,
                profile?.Sector ?? string.Empty
            ),
            new BmcProjectContext(
                project.Title,
                project.Description,
                project.Stage.ToString()
            )
        );

        var bmcResult = await _aiService.GenerateBmcAsync(aiInput);

        try 
        {
            var bmcData = ExtractBmcResponse(bmcResult);

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
        catch (Exception ex)
        {
            Console.WriteLine($"Error persisting BMC: {ex.Message}");
        }

        return bmcResult;
    }

    private static void MapDtoToEntity(BmcResponseDto dto, ProjectBmc entity)
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

    private static BmcResponseDto ExtractBmcResponse(object bmcResult)
    {
        var root = bmcResult is JsonElement element
            ? element
            : JsonSerializer.SerializeToElement(bmcResult);

        var canvas = TryUnwrapCanvas(root);

        return new BmcResponseDto
        {
            CustomerSegments = ReadSection(canvas, "customer_segments"),
            ValueProposition = ReadSection(canvas, "value_proposition"),
            Channels = ReadSection(canvas, "channels"),
            CustomerRelationships = ReadSection(canvas, "customer_relationships"),
            RevenueStreams = ReadSection(canvas, "revenue_streams"),
            KeyResources = ReadSection(canvas, "key_resources"),
            KeyActivities = ReadSection(canvas, "key_activities"),
            KeyPartners = ReadSection(canvas, "key_partners"),
            CostStructure = ReadSection(canvas, "cost_structure")
        };
    }

    private static JsonElement TryUnwrapCanvas(JsonElement root)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            return root;
        }

        foreach (var propertyName in new[] { "bmc", "data", "result", "response", "canvas" })
        {
            if (TryGetPropertyIgnoreCase(root, propertyName, out var nested) && nested.ValueKind == JsonValueKind.Object)
            {
                return nested;
            }
        }

        return root;
    }

    private static string? ReadSection(JsonElement root, string propertyName)
    {
        if (!TryGetPropertyIgnoreCase(root, propertyName, out var section) ||
            section.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return string.Empty;
        }

        return section.ValueKind switch
        {
            JsonValueKind.Array => JsonSerializer.Serialize(ReadStringList(section)),
            JsonValueKind.String => NormalizeStringSection(section.GetString()),
            JsonValueKind.Object => section.GetRawText(),
            JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False => section.GetRawText(),
            _ => string.Empty
        };
    }

    private static string NormalizeStringSection(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim();

        if (trimmed.StartsWith('[') && trimmed.EndsWith(']'))
        {
            try
            {
                var items = JsonSerializer.Deserialize<List<string>>(trimmed);
                if (items is { Count: > 0 })
                {
                    return JsonSerializer.Serialize(items);
                }
            }
            catch
            {
                return value;
            }
        }

        return value;
    }

    private static List<string> ReadStringList(JsonElement section)
    {
        var values = new List<string>();

        foreach (var item in section.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.String)
            {
                var value = item.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    values.Add(value);
                }
                continue;
            }

            var raw = item.GetRawText().Trim('"');
            if (!string.IsNullOrWhiteSpace(raw))
            {
                values.Add(raw);
            }
        }

        return values;
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement value)
    {
        if (element.TryGetProperty(propertyName, out value))
        {
            return true;
        }

        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
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

    private sealed record BmcGenerationRequest(
        Guid EntrepreneurId,
        Guid ProjectId,
        BmcEntrepreneurContext Entrepreneur,
        BmcProjectContext Project
    );

    private sealed record BmcEntrepreneurContext(
        string Bio,
        List<string> Skills,
        List<string> Industries,
        int ExperienceYears,
        string ExperienceLevel,
        string Sector
    );

    private sealed record BmcProjectContext(
        string Title,
        string Description,
        string Stage
    );
}