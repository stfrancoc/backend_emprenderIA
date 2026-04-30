using MediatR;
using EmprendeIA.Domain.Interfaces;
using System.Text.Json;

namespace EmprendeIA.Application.Projects.GetBmc;

public class GetProjectBmcQueryHandler : IRequestHandler<GetProjectBmcQuery, ProjectBmcDto?>
{
    private readonly IProjectBmcRepository _repository;

    public GetProjectBmcQueryHandler(IProjectBmcRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProjectBmcDto?> Handle(GetProjectBmcQuery request, CancellationToken cancellationToken)
    {
        var bmc = await _repository.GetByProjectIdAsync(request.ProjectId);
        if (bmc == null) return null;

        return new ProjectBmcDto(
            bmc.ProjectId,
            ParseList(bmc.CustomerSegments),
            ParseList(bmc.ValueProposition),
            ParseList(bmc.Channels),
            ParseList(bmc.CustomerRelationships),
            ParseList(bmc.RevenueStreams),
            ParseList(bmc.KeyResources),
            ParseList(bmc.KeyActivities),
            ParseList(bmc.KeyPartners),
            ParseList(bmc.CostStructure),
            bmc.UpdatedAt
        );
    }

    private static List<string> ParseList(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return new List<string>();
        
        try 
        {
            return JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>();
        }
        catch 
        {
            return value.Split(new[] { '\n', ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(s => s.Trim())
                       .ToList();
        }
    }
}
