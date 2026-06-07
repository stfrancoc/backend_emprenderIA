using MediatR;
using System.Text.Json.Serialization;

namespace EmprendeIA.Application.Projects.Create;

using EmprendeIA.Domain.Projects;

public record CreateProjectCommand(
    string Title, 
    string Description, 
    string What, 
    string How, 
    string Why, 
    ProjectType ProjectType, 
    BusinessModelType BusinessModelType
) : IRequest<Guid>
{
    [JsonIgnore] // Esto hace que no aparezca en Swagger, lo llenamos internamente
    public Guid OwnerId { get; set; }
}