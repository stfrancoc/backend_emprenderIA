using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;
using System.Text.Json;

namespace EmprendeIA.Application.Projects.UpdateBmc;

public class UpdateBmcCommandHandler : IRequestHandler<UpdateBmcCommand, bool>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectBmcRepository _bmcRepository;

    public UpdateBmcCommandHandler(IProjectRepository projectRepository, IProjectBmcRepository bmcRepository)
    {
        _projectRepository = projectRepository;
        _bmcRepository = bmcRepository;
    }

    public async Task<bool> Handle(UpdateBmcCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project == null || project.OwnerId != request.UserId)
        {
            return false;
        }

        var bmc = await _bmcRepository.GetByProjectIdAsync(request.ProjectId);

        if (bmc == null)
        {
            // If it doesn't exist, we create it
            bmc = new ProjectBmc(request.ProjectId);
            bmc.UpdateCanvas(
                JsonSerializer.Serialize(request.CustomerSegments),
                JsonSerializer.Serialize(request.ValueProposition),
                JsonSerializer.Serialize(request.Channels),
                JsonSerializer.Serialize(request.CustomerRelationships),
                JsonSerializer.Serialize(request.RevenueStreams),
                JsonSerializer.Serialize(request.KeyResources),
                JsonSerializer.Serialize(request.KeyActivities),
                JsonSerializer.Serialize(request.KeyPartners),
                JsonSerializer.Serialize(request.CostStructure)
            );
            await _bmcRepository.AddAsync(bmc);
        }
        else
        {
            bmc.UpdateCanvas(
                JsonSerializer.Serialize(request.CustomerSegments),
                JsonSerializer.Serialize(request.ValueProposition),
                JsonSerializer.Serialize(request.Channels),
                JsonSerializer.Serialize(request.CustomerRelationships),
                JsonSerializer.Serialize(request.RevenueStreams),
                JsonSerializer.Serialize(request.KeyResources),
                JsonSerializer.Serialize(request.KeyActivities),
                JsonSerializer.Serialize(request.KeyPartners),
                JsonSerializer.Serialize(request.CostStructure)
            );
            await _bmcRepository.UpdateAsync(bmc);
        }

        return true;
    }
}

