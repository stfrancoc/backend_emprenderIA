using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/resources")]
[Authorize]
public class ResourcesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetResources([FromQuery] string? stage)
    {
        var resources = new List<object>
        {
            new { id = 1, type = "course", title = "Emprendimiento 101", category = "Fundamentos", stage = "idea", url = "https://example.com/curso-101" },
            new { id = 2, type = "template", title = "Business Plan Template", category = "Documentos", stage = "validation", url = "https://example.com/template-bp" },
            new { id = 3, type = "guide", title = "Guía de Marketing Digital", category = "Marketing", stage = "launch", url = "https://example.com/guide-marketing" },
            new { id = 4, type = "tool", title = "Financial Calculator", category = "Finanzas", stage = "growth", url = "https://example.com/calc-finance" },
            new { id = 5, type = "course", title = "Customer Development", category = "Validación", stage = "validation", url = "https://example.com/customer-dev" },
            new { id = 6, type = "template", title = "Pitch Deck Template", category = "Presentaciones", stage = "fundraising", url = "https://example.com/template-pitch" },
            new { id = 7, type = "guide", title = "Gestión de Equipos", category = "Operaciones", stage = "growth", url = "https://example.com/guide-teams" },
            new { id = 8, type = "course", title = "Fundamentos de Producto", category = "Producto", stage = "validation", url = "https://example.com/product-basics" }
        };

        if (!string.IsNullOrEmpty(stage))
        {
            resources = resources.Where(r => r.GetType().GetProperty("stage")?.GetValue(r)?.ToString() == stage).ToList();
        }

        return Ok(resources);
    }
}
