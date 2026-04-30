using MediatR;

namespace EmprendeIA.Application.Projects.GetBmc;

public record GetProjectBmcQuery(Guid ProjectId) : IRequest<ProjectBmcDto?>;

public record ProjectBmcDto(
    Guid ProjectId,
    List<string> CustomerSegments,
    List<string> ValueProposition,
    List<string> Channels,
    List<string> CustomerRelationships,
    List<string> RevenueStreams,
    List<string> KeyResources,
    List<string> KeyActivities,
    List<string> KeyPartners,
    List<string> CostStructure,
    DateTime UpdatedAt
);
