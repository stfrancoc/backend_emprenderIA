using MediatR;
using EmprendeIA.Application.Marketplace.Queries.GetMatches;

namespace EmprendeIA.Application.Marketplace.Queries.GetProjectMatches;

public record GetProjectMatchesQuery(Guid ProjectId, Guid RequestingUserId) : IRequest<List<MatchDto>>;
