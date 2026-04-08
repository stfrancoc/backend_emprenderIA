using MediatR;
using Microsoft.AspNetCore.Mvc;
using EmprendeIA.Application.Projects.Create;
using EmprendeIA.Application.Projects.GetByUser;
using EmprendeIA.Application.Projects.GetById;
using EmprendeIA.Application.Projects.Update;
using EmprendeIA.Application.Projects.Delete;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmprendeIA.Application.Projects.GenerateBmc;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectCommand command)
    {
        command.OwnerId = GetUserId();
        var projectId = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = projectId },
            new { id = projectId }
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProjects()
    {
        var query = new GetUserProjectsQuery(GetUserId());
        var projects = await _mediator.Send(query);
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var project = await _mediator.Send(new GetProjectByIdQuery(id));

        if (project == null) return NotFound();

        if (project.OwnerId != GetUserId())
        {
            return Forbid();
        }

        return Ok(project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProjectCommand command)
    {
        if (id != command.Id) return BadRequest("ID de la URL no coincide con el del cuerpo");

        command.OwnerId = GetUserId();
        var result = await _mediator.Send(command);

        if (!result) return NotFound("Proyecto no encontrado o no tienes permiso");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProjectCommand(id, GetUserId()));

        if (!result) return NotFound("Proyecto no encontrado o no tienes permiso");

        return NoContent();
    }

    [HttpPost("{id}/generate-bmc")]
    public async Task<IActionResult> GenerateBmc(Guid id)
    {
        var result = await _mediator.Send(new GenerateBmcCommand(id, GetUserId()));
        return Ok(result);
    }
}