using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Marketplace.Queries.GetMatches;

public class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, List<MatchDto>>
{
    private readonly IProjectMatchRepository _matchRepository;

    public GetMatchesQueryHandler(IProjectMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<List<MatchDto>> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
    {
        var matches = await _matchRepository.GetMatchesByUserAsync(request.UserId);
        
        return matches.Select(m => new MatchDto(
            m.Id,
            m.ProjectId,
            m.Project.Title,
            m.InvestorId,
            "Inversor",
            m.MatchScore,
            m.CreatedAt
        )).ToList();
    }
}
