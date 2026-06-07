using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmprendeIA.Application.Marketplace.Commands.CreateMatch;
using EmprendeIA.Application.Marketplace.Commands.GenerateProjectMatches;
using EmprendeIA.Application.Marketplace.Queries.GetMatches;
using EmprendeIA.Application.Marketplace.Queries.GetProjectMatches;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/Projects/[controller]")]
[Authorize]
public class MatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    // POST api/Projects/Matches — Create a single manual match
    [HttpPost]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
    {
        var command = new CreateMatchCommand(request.ProjectId, GetUserId());
        var matchId = await _mediator.Send(command);

        if (matchId == null)
            return BadRequest("No se pudo crear el match o no tienes permiso.");

        return Ok(new { id = matchId });
    }

    // POST api/Projects/Matches/generate — Invoke AI engine to calculate & persist matches for a project
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateMatches([FromBody] GenerateMatchesRequest request)
    {
        var command = new GenerateProjectMatchesCommand(request.ProjectId, GetUserId(), request.BmcText);
        var matchIds = await _mediator.Send(command);

        if (matchIds.Count == 0)
            return Ok(new { message = "No se encontraron matches con score ≥ 70%.", matches = matchIds });

        return Ok(new { message = $"{matchIds.Count} match(es) generados y guardados.", matches = matchIds });
    }

    // GET api/Projects/Matches — Get matches for the authenticated user (across all projects)
    [HttpGet]
    public async Task<IActionResult> GetMatches()
    {
        var query = new GetMatchesQuery(GetUserId());
        var matches = await _mediator.Send(query);
        return Ok(matches);
    }

    // GET api/Projects/Matches/{projectId} — Get matches for a specific project
    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> GetProjectMatches(Guid projectId)
    {
        var query = new GetProjectMatchesQuery(projectId, GetUserId());
        var matches = await _mediator.Send(query);
        return Ok(matches);
    }
}

public class CreateMatchRequest
{
    public Guid ProjectId { get; set; }
}

public class GenerateMatchesRequest
{
    public Guid ProjectId { get; set; }
    public string BmcText { get; set; } = string.Empty;
}
