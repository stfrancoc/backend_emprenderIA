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
using EmprendeIA.Application.Projects.GenerateFinancialAnalysis;
using EmprendeIA.Application.Projects.GetBmc;
using EmprendeIA.Application.Projects.UpdateBmc;
using EmprendeIA.Application.Projects.UploadDocument;
using EmprendeIA.Application.Projects.GenerateBusinessPlan;
using EmprendeIA.Application.Projects.GetBusinessPlan;
using EmprendeIA.Application.Projects.UpdateBusinessPlan;


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

    [HttpPost("{id}/generate-financial")]
    public async Task<IActionResult> GenerateFinancial(Guid id)
    {
        var result = await _mediator.Send(new GenerateFinancialAnalysisCommand(id, GetUserId()));
        return Ok(result);
    }

    [HttpGet("{id}/bmc")]
    public async Task<IActionResult> GetBmc(Guid id)
    {
        var bmc = await _mediator.Send(new GetProjectBmcQuery(id));
        if (bmc == null) return NotFound();
        return Ok(bmc);
    }

    [HttpPut("{id}/bmc")]
    public async Task<IActionResult> UpdateBmc(Guid id, UpdateBmcCommand command)
    {
        if (id != command.ProjectId) return BadRequest("ID Mismatch");
        
        var result = await _mediator.Send(command with { UserId = GetUserId() });
        if (!result) return NotFound("Proyecto no encontrado o no tienes permiso");
        
        return NoContent();
    }

    // ── Document Upload ──────────────────────────────────────────────
    [HttpPost("{id}/documents")]
    [RequestSizeLimit(20 * 1024 * 1024)] // 20 MB max
    public async Task<IActionResult> UploadDocument(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se proporcionó un archivo válido.");

        var allowed = new[] { ".pdf", ".txt", ".docx" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowed.Contains(ext))
            return BadRequest("Formato no soportado. Use PDF, TXT o DOCX.");

        await using var stream = file.OpenReadStream();
        var docId = await _mediator.Send(new UploadProjectDocumentCommand(
            id, GetUserId(), stream, file.FileName));

        return Ok(new { documentId = docId, message = "Documento indexado correctamente en el asistente IA." });
    }

    // ── Business Plan ────────────────────────────────────────────────
    [HttpGet("{id}/business-plan")]
    public async Task<IActionResult> GetBusinessPlan(Guid id)
    {
        var content = await _mediator.Send(new GetBusinessPlanQuery(id));
        if (content == null) return NotFound();
        return Ok(new { content });
    }

    [HttpPost("{id}/business-plan/generate")]
    public async Task<IActionResult> GenerateBusinessPlan(Guid id)
    {
        var content = await _mediator.Send(new GenerateBusinessPlanCommand(id, GetUserId()));
        return Ok(new { content });
    }

    [HttpPut("{id}/business-plan")]
    public async Task<IActionResult> UpdateBusinessPlan(Guid id, [FromBody] UpdateBusinessPlanCommand command)
    {
        if (id != command.ProjectId) return BadRequest("ID Mismatch");
        var result = await _mediator.Send(command with { UserId = GetUserId() });
        if (!result) return NotFound("Proyecto no encontrado o sin permisos.");
        return NoContent();
    }
}