using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmprendeIA.Application.Projects.GenerateFinancialAnalysis;
using EmprendeIA.Application.Projects.UpdateFinancialAnalysis;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FinancialController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFinancialRepository _financialRepository;

    public FinancialController(IMediator mediator, IFinancialRepository financialRepository)
    {
        _mediator = mediator;
        _financialRepository = financialRepository;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost("projects/{projectId}/generate")]
    public async Task<IActionResult> Generate(Guid projectId)
    {
        var command = new GenerateFinancialAnalysisCommand(projectId, GetUserId());
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("projects/{projectId}")]
    public async Task<IActionResult> GetByProject(Guid projectId)
    {
        var analysis = await _financialRepository.GetByProjectIdAsync(projectId);
        if (analysis == null) return NotFound();
        
        return Ok(analysis);
    }

    [HttpPut("projects/{projectId}")]
    public async Task<IActionResult> Update(Guid projectId, UpdateFinancialAnalysisCommand command)
    {
        if (projectId != command.ProjectId) return BadRequest("ID Mismatch");
        
        var result = await _mediator.Send(command with { UserId = GetUserId() });
        if (!result) return NotFound("Análisis no encontrado o no tienes permiso");
        
        return NoContent();
    }
}
