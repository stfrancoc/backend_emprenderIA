using MediatR;
using System.Text.Json.Serialization;
using EmprendeIA.Domain.Entities.Marketplace;

namespace EmprendeIA.Application.Marketplace.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    ProductCategory Category,
    decimal Price,
    List<string> Images,
    bool Visibility
) : IRequest<bool>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}
