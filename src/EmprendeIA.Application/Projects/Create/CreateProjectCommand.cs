using MediatR;
using System.Text.Json.Serialization;

namespace EmprendeIA.Application.Projects.Create;

public record CreateProjectCommand(string Title, string Description) : IRequest<Guid>
{
    [JsonIgnore] // Esto hace que no aparezca en Swagger, lo llenamos internamente
    public Guid OwnerId { get; set; }
}