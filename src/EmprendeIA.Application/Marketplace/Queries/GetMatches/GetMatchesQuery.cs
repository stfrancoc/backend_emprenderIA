using MediatR;
using System;
using System.Collections.Generic;

namespace EmprendeIA.Application.Marketplace.Queries.GetMatches;

public record GetMatchesQuery(Guid UserId) : IRequest<List<MatchDto>>;
