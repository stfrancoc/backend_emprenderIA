using MediatR;

namespace EmprendeIA.Application.Projects.GenerateBusinessPlan;

public record GenerateBusinessPlanCommand(Guid ProjectId, Guid UserId) : IRequest<string>;
