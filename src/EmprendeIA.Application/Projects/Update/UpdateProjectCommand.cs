using MediatR;
using System.Text.Json.Serialization;

namespace EmprendeIA.Application.Projects.Update;

public record UpdateProjectCommand(
    Guid Id,
    string Title,
    string Description
) : IRequest<bool>
{
    [JsonIgnore]
    public Guid OwnerId { get; set; }
}