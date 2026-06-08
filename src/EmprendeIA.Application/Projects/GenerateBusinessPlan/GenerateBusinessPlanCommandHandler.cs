using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Application.Projects.GenerateBusinessPlan;

public class GenerateBusinessPlanCommandHandler : IRequestHandler<GenerateBusinessPlanCommand, string>
{
    private readonly IAIService _aiService;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectBmcRepository _bmcRepository;
    private readonly IBusinessPlanRepository _businessPlanRepository;

    public GenerateBusinessPlanCommandHandler(
        IAIService aiService,
        IProjectRepository projectRepository,
        IProjectBmcRepository bmcRepository,
        IBusinessPlanRepository businessPlanRepository)
    {
        _aiService = aiService;
        _projectRepository = projectRepository;
        _bmcRepository = bmcRepository;
        _businessPlanRepository = businessPlanRepository;
    }

    public async Task<string> Handle(GenerateBusinessPlanCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.UserId)
            throw new Exception("Proyecto no encontrado o sin permisos.");

        var bmc = await _bmcRepository.GetByProjectIdAsync(request.ProjectId);

        // Build a textual summary of the BMC to send to the AI
        var bmcText = bmc != null
            ? $"Propuesta de Valor: {bmc.ValueProposition}\n" +
              $"Segmentos: {bmc.CustomerSegments}\n" +
              $"Canales: {bmc.Channels}\n" +
              $"Relaciones con Clientes: {bmc.CustomerRelationships}\n" +
              $"Fuentes de Ingreso: {bmc.RevenueStreams}\n" +
              $"Recursos Clave: {bmc.KeyResources}\n" +
              $"Actividades Clave: {bmc.KeyActivities}\n" +
              $"Aliados: {bmc.KeyPartners}\n" +
              $"Estructura de Costos: {bmc.CostStructure}"
            : $"Proyecto: {project.Title}. Descripción: {project.Description}.";

        var markdown = await _aiService.GenerateBusinessPlanMarkdownAsync(bmcText);

        var existing = await _businessPlanRepository.GetByProjectIdAsync(request.ProjectId);
        if (existing == null)
        {
            var plan = new BusinessPlan(request.ProjectId, markdown);
            await _businessPlanRepository.AddAsync(plan);
        }
        else
        {
            existing.UpdateContent(markdown);
            await _businessPlanRepository.UpdateAsync(existing);
        }

        return markdown;
    }
}
