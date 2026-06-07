using MediatR;

namespace EmprendeIA.Application.Projects.UpdateFinancialAnalysis;

public record UpdateFinancialAnalysisCommand(
    Guid ProjectId,
    Guid UserId,
    string RevenueProjections,
    string CostAnalysis,
    string BreakEvenAnalysis,
    string FundingRequirements,
    string KeyIndicators,
    decimal NetPresentValue,
    decimal InternalRateOfReturn,
    int BreakEvenUnits,
    bool IsViable
) : IRequest<bool>;
