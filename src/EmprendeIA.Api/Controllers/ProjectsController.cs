using MediatR;
using Microsoft.AspNetCore.Mvc;
using EmprendeIA.Application.Projects.Create;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectCommand command)
    {
        var projectId = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = projectId },
            projectId
        );
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok();
    }
}