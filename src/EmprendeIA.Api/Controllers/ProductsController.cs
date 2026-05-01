using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmprendeIA.Application.Marketplace.Commands.CreateProduct;
using EmprendeIA.Application.Marketplace.Commands.UpdateProduct;
using EmprendeIA.Application.Marketplace.Commands.DeleteProduct;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        command.UserId = GetUserId();
        var productId = await _mediator.Send(command);

        if (productId == null)
        {
            return Forbid("No tienes permiso para crear productos en este proyecto o el proyecto no existe.");
        }

        return Ok(new { id = productId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        command.UserId = GetUserId();
        var result = await _mediator.Send(command);

        if (!result) return NotFound("Producto no encontrado o no tienes permiso");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id, GetUserId()));

        if (!result) return NotFound("Producto no encontrado o no tienes permiso");

        return NoContent();
    }
}
