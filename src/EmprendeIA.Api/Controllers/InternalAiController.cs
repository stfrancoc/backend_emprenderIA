using System.Text.Json;
using System.Text.Json.Serialization;
using EmprendeIA.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[EmprendeIA.Api.Swagger.InternalApiKey]
[Route("internal/ai")]
public class InternalAiController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectBmcRepository _projectBmcRepository;

    public InternalAiController(
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IProjectBmcRepository projectBmcRepository)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _projectBmcRepository = projectBmcRepository;
    }

    [HttpGet("users/{userId:guid}")]
    [ProducesResponseType(typeof(InternalUserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InternalUserProfileResponse>> GetUserProfileById([FromRoute] Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var profile = user.UserProfile;

        var response = new InternalUserProfileResponse(
            profile?.Bio ?? string.Empty,
            profile?.Skills ?? new List<string>(),
            profile?.Industries ?? new List<string>(),
            profile?.ExperienceLevel ?? string.Empty
        );

        return Ok(response);
    }

    [HttpGet("projects/{projectId:guid}")]
    [ProducesResponseType(typeof(InternalProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InternalProjectResponse>> GetProjectById([FromRoute] Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            return NotFound();
        }

        var response = new InternalProjectResponse(
            project.Title,
            project.Description,
            project.Stage.ToString()
        );

        return Ok(response);
    }

    [HttpGet("projects/{projectId:guid}/bmc")]
    [ProducesResponseType(typeof(InternalProjectBmcResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InternalProjectBmcResponse>> GetProjectBmcByProjectId([FromRoute] Guid projectId)
    {
        var bmc = await _projectBmcRepository.GetByProjectIdAsync(projectId);
        if (bmc == null)
        {
            return NotFound();
        }

        var response = new InternalProjectBmcResponse(
            ParseToStringList(bmc.CustomerSegments),
            ParseToStringList(bmc.ValueProposition),
            ParseToStringList(bmc.Channels),
            ParseToStringList(bmc.CustomerRelationships),
            ParseToStringList(bmc.RevenueStreams),
            ParseToStringList(bmc.KeyResources),
            ParseToStringList(bmc.KeyActivities),
            ParseToStringList(bmc.KeyPartners),
            ParseToStringList(bmc.CostStructure)
        );

        return Ok(response);
    }

    private static List<string> ParseToStringList(string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return new List<string>();
        }

        try
        {
            var fromJson = JsonSerializer.Deserialize<List<string>>(rawValue);
            if (fromJson is { Count: > 0 })
            {
                return fromJson;
            }
        }
        catch
        {
            // Fall back to delimiter parsing if value is not a valid JSON array.
        }

        var parts = rawValue
            .Split(new[] { '\n', ';', ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToList();

        return parts;
    }
}

public record InternalUserProfileResponse(
    [property: JsonPropertyName("bio")] string Bio,
    [property: JsonPropertyName("skills")] List<string> Skills,
    [property: JsonPropertyName("industries")] List<string> Industries,
    [property: JsonPropertyName("experience")] string Experience
);

public record InternalProjectResponse(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("stage")] string Stage
);

public record InternalProjectBmcResponse(
    [property: JsonPropertyName("customer_segments")] List<string> CustomerSegments,
    [property: JsonPropertyName("value_proposition")] List<string> ValueProposition,
    [property: JsonPropertyName("channels")] List<string> Channels,
    [property: JsonPropertyName("customer_relationships")] List<string> CustomerRelationships,
    [property: JsonPropertyName("revenue_streams")] List<string> RevenueStreams,
    [property: JsonPropertyName("key_resources")] List<string> KeyResources,
    [property: JsonPropertyName("key_activities")] List<string> KeyActivities,
    [property: JsonPropertyName("key_partners")] List<string> KeyPartners,
    [property: JsonPropertyName("cost_structure")] List<string> CostStructure
);