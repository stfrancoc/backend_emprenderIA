namespace EmprendeIA.Domain.Interfaces;

public record MatchResultItem(string MatchedUserId, string Role, double MatchScore);
public record CalculateMatchesResponse(string ProjectId, List<MatchResultItem> Matches);
