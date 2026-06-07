using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Application.Marketplace.Commands.GenerateProjectMatches;

public class GenerateProjectMatchesCommandHandler : IRequestHandler<GenerateProjectMatchesCommand, List<Guid>>
{
    private const double MinScoreThreshold = 70.0;

    private readonly IProjectMatchRepository _matchRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IAIService _aiService;
    private readonly INotificationService _notificationService;

    public GenerateProjectMatchesCommandHandler(
        IProjectMatchRepository matchRepository,
        IProjectRepository projectRepository,
        IAIService aiService,
        INotificationService notificationService)
    {
        _matchRepository = matchRepository;
        _projectRepository = projectRepository;
        _aiService = aiService;
        _notificationService = notificationService;
    }

    public async Task<List<Guid>> Handle(GenerateProjectMatchesCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.RequestingUserId)
            return [];

        var aiResponse = await _aiService.CalculateMatchesAsync(
            request.ProjectId.ToString(),
            request.BmcText);

        if (aiResponse == null || aiResponse.Matches.Count == 0)
            return [];

        var savedIds = new List<Guid>();

        foreach (var m in aiResponse.Matches.Where(m => m.MatchScore >= MinScoreThreshold))
        {
            if (!Guid.TryParse(m.MatchedUserId, out var matchedUserId))
                continue;

            // Skip if already matched
            var existing = await _matchRepository.GetMatchAsync(request.ProjectId, matchedUserId);
            if (existing != null)
            {
                savedIds.Add(existing.Id);
                continue;
            }

            var match = new ProjectMatch(request.ProjectId, matchedUserId, (decimal)m.MatchScore);
            await _matchRepository.AddAsync(match);
            savedIds.Add(match.Id);

            // Send real-time notification to the user who requested the matches
            await _notificationService.SendMatchNotificationAsync(request.RequestingUserId, request.ProjectId, project.Title, (decimal)m.MatchScore);
        }

        return savedIds;
    }
}
