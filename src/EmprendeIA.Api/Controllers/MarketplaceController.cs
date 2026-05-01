using MediatR;
using Microsoft.AspNetCore.Mvc;
using EmprendeIA.Application.Marketplace.Queries.GetMarketplace;

namespace EmprendeIA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketplaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public MarketplaceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMarketplace()
    {
        var products = await _mediator.Send(new GetMarketplaceQuery());
        return Ok(products);
    }
}
