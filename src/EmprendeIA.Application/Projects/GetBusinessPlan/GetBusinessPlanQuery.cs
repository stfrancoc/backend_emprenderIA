using MediatR;

namespace EmprendeIA.Application.Projects.GetBusinessPlan;

public record GetBusinessPlanQuery(Guid ProjectId) : IRequest<string?>;
