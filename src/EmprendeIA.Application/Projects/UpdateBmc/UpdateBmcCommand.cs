using MediatR;

namespace EmprendeIA.Application.Projects.UpdateBmc;

public record UpdateBmcCommand(
    Guid ProjectId,
    Guid UserId,
    List<string> CustomerSegments,
    List<string> ValueProposition,
    List<string> Channels,
    List<string> CustomerRelationships,
    List<string> RevenueStreams,
    List<string> KeyResources,
    List<string> KeyActivities,
    List<string> KeyPartners,
    List<string> CostStructure
) : IRequest<bool>;
