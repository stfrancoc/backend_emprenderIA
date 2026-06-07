using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Application.Marketplace.Queries.GetMatches;

namespace EmprendeIA.Application.Marketplace.Queries.GetProjectMatches;

public class GetProjectMatchesQueryHandler : IRequestHandler<GetProjectMatchesQuery, List<MatchDto>>
{
    private readonly IProjectMatchRepository _matchRepository;
    private readonly IProjectRepository _projectRepository;

    public GetProjectMatchesQueryHandler(
        IProjectMatchRepository matchRepository,
        IProjectRepository projectRepository)
    {
        _matchRepository = matchRepository;
        _projectRepository = projectRepository;
    }

    public async Task<List<MatchDto>> Handle(GetProjectMatchesQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.RequestingUserId)
            return [];

        var matches = await _matchRepository.GetMatchesByProjectAsync(request.ProjectId);

        return matches.Select(m => new MatchDto(
            m.Id,
            m.ProjectId,
            m.Project.Title,
            m.InvestorId,
            "Matched Profile",
            m.MatchScore,
            m.CreatedAt
        )).ToList();
    }
}
