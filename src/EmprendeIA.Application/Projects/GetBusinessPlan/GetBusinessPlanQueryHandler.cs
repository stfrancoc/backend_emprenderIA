using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Projects.GetBusinessPlan;

public class GetBusinessPlanQueryHandler : IRequestHandler<GetBusinessPlanQuery, string?>
{
    private readonly IBusinessPlanRepository _repository;

    public GetBusinessPlanQueryHandler(IBusinessPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> Handle(GetBusinessPlanQuery request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByProjectIdAsync(request.ProjectId);
        return plan?.Content;
    }
}
