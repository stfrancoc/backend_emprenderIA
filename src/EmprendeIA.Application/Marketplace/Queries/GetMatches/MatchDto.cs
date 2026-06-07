using System;

namespace EmprendeIA.Application.Marketplace.Queries.GetMatches;

public record MatchDto(
    Guid Id,
    Guid ProjectId,
    string ProjectTitle,
    Guid InvestorId,
    string InvestorName,
    decimal MatchScore,
    DateTime CreatedAt
);
