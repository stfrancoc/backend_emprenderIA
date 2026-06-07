using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;
using System.Text.Json;

namespace EmprendeIA.Application.Projects.GenerateFinancialAnalysis;

public class GenerateFinancialAnalysisCommandHandler : IRequestHandler<GenerateFinancialAnalysisCommand, object>
{
    private readonly IAIService _aiService;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectBmcRepository _bmcRepository;
    private readonly IFinancialRepository _financialRepository;

    public GenerateFinancialAnalysisCommandHandler(
        IAIService aiService, 
        IProjectRepository projectRepository,
        IProjectBmcRepository bmcRepository,
        IFinancialRepository financialRepository)
    {
        _aiService = aiService;
        _projectRepository = projectRepository;
        _bmcRepository = bmcRepository;
        _financialRepository = financialRepository;
    }

    public async Task<object> Handle(GenerateFinancialAnalysisCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.UserId) 
            throw new Exception("Proyecto no encontrado");

        var bmc = await _bmcRepository.GetByProjectIdAsync(request.ProjectId);
        if (bmc == null)
            throw new Exception("Debes generar el BMC primero para poder realizar el análisis financiero.");

        var aiInput = new 
        {
            entrepreneur_id = request.UserId.ToString(),
            project_id = request.ProjectId.ToString(),
            bmc_id = bmc.ProjectId.ToString()
        };

        var aiResult = await _aiService.GenerateFinancialAnalysisAsync(aiInput);

        try 
        {
            // Extraer datos y persistir
            var resultElement = aiResult is JsonElement element ? element : JsonSerializer.SerializeToElement(aiResult);
            
            // Navegar hasta financial_analysis
            if (resultElement.TryGetProperty("financial_analysis", out var analysis))
            {
                var existing = await _financialRepository.GetByProjectIdAsync(project.Id);
                if (existing == null)
                {
                    existing = new ProjectFinancialAnalysis(project.Id);
                    UpdateEntity(existing, analysis);
                    await _financialRepository.AddAsync(existing);
                }
                else
                {
                    UpdateEntity(existing, analysis);
                    await _financialRepository.UpdateAsync(existing);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error persistiendo análisis financiero: {ex.Message}");
        }

        return aiResult;
    }

    private void UpdateEntity(ProjectFinancialAnalysis entity, JsonElement analysis)
    {
        entity.Update(
            analysis.GetProperty("revenue_projections").GetString() ?? "",
            analysis.GetProperty("cost_analysis").GetString() ?? "",
            analysis.GetProperty("break_even_analysis").GetString() ?? "",
            analysis.GetProperty("funding_requirements").GetString() ?? "",
            analysis.GetProperty("key_financial_indicators").GetString() ?? "",
            analysis.TryGetProperty("net_present_value", out var npv) ? npv.GetDecimal() : 0m,
            analysis.TryGetProperty("internal_rate_of_return", out var irr) ? irr.GetDecimal() : 0m,
            analysis.TryGetProperty("break_even_units", out var beu) ? beu.GetInt32() : 0,
            analysis.TryGetProperty("is_viable", out var iv) && iv.GetBoolean()
        );
    }
}
