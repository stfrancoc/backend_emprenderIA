using MediatR;
using System.Text.Json.Serialization;

namespace EmprendeIA.Application.Projects.UpdateBusinessPlan;

public record UpdateBusinessPlanCommand(Guid ProjectId, string Content) : IRequest<bool>
{
    [JsonIgnore]
    public Guid UserId { get; init; }
}
