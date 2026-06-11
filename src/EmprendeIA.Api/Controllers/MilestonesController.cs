using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmprendeIA.Domain.Entities;
using EmprendeIA.Domain.Interfaces;
using System.Security.Claims;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId}/milestones")]
[Authorize]
public class MilestonesController : ControllerBase
{
    private readonly IMilestoneRepository _repository;
    private readonly IProjectRepository _projectRepository;

    public MilestonesController(IMilestoneRepository repository, IProjectRepository projectRepository)
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMilestones(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            return NotFound("Project not found");

        var milestones = await _repository.GetByProjectIdAsync(projectId);
        return Ok(milestones);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMilestone(Guid projectId, [FromBody] CreateMilestoneRequest request)
    {
        if (request == null)
            return BadRequest("Solicitud de hito inválida.");

        var effectiveProjectId = request.ProjectId != Guid.Empty ? request.ProjectId : projectId;
        if (effectiveProjectId != projectId)
            return BadRequest("El projectId de la ruta y del cuerpo no coinciden.");

        var project = await _projectRepository.GetByIdAsync(effectiveProjectId);
        if (project == null)
            return NotFound("Project not found");

        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("El título del hito es obligatorio.");

        var dueDate = request.DueDate.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(request.DueDate, DateTimeKind.Utc)
            : request.DueDate;

        var milestone = new Milestone(effectiveProjectId, request.Title.Trim(), request.Description?.Trim() ?? string.Empty, dueDate);
        await _repository.AddAsync(milestone);
        return CreatedAtAction(nameof(GetMilestones), new { projectId = effectiveProjectId }, milestone);
    }

    [HttpPut("{milestoneId}/toggle")]
    public async Task<IActionResult> ToggleMilestone(Guid projectId, Guid milestoneId)
    {
        var milestone = await _repository.GetByIdAsync(milestoneId);
        if (milestone == null)
            return NotFound("Milestone not found");

        if (milestone.ProjectId != projectId)
            return BadRequest("Milestone does not belong to this project");

        milestone.ToggleCompletion();
        await _repository.UpdateAsync(milestone);
        return Ok(milestone);
    }
}

public class CreateMilestoneRequest
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}
