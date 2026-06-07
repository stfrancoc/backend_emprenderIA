using MediatR;
using System.Text.Json.Serialization;

namespace EmprendeIA.Application.Projects.Update;

using EmprendeIA.Domain.Projects;

public record UpdateProjectCommand(
    Guid Id,
    string Title,
    string Description,
    string What,
    string How,
    string Why,
    ProjectType ProjectType,
    BusinessModelType BusinessModelType
) : IRequest<bool>
{
    [JsonIgnore]
    public Guid OwnerId { get; set; }
}