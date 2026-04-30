using MediatR;

namespace EmprendeIA.Application.Projects.GenerateFinancialAnalysis;

public record GenerateFinancialAnalysisCommand(Guid ProjectId, Guid UserId) : IRequest<object>;
