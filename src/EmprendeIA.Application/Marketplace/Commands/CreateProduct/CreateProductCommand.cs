using MediatR;
using System.Text.Json.Serialization;
using EmprendeIA.Domain.Entities.Marketplace;

namespace EmprendeIA.Application.Marketplace.Commands.CreateProduct;

public record CreateProductCommand(
    Guid ProjectId,
    string Name,
    string Description,
    ProductCategory Category,
    decimal Price,
    List<string>? Images
) : IRequest<Guid?>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}
