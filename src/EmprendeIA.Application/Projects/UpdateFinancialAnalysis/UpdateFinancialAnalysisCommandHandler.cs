using MediatR;
using EmprendeIA.Domain.Interfaces;
using EmprendeIA.Domain.Entities;

namespace EmprendeIA.Application.Projects.UpdateFinancialAnalysis;

public class UpdateFinancialAnalysisCommandHandler : IRequestHandler<UpdateFinancialAnalysisCommand, bool>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IFinancialRepository _financialRepository;

    public UpdateFinancialAnalysisCommandHandler(IProjectRepository projectRepository, IFinancialRepository financialRepository)
    {
        _projectRepository = projectRepository;
        _financialRepository = financialRepository;
    }

    public async Task<bool> Handle(UpdateFinancialAnalysisCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project == null || project.OwnerId != request.UserId)
        {
            return false;
        }

        var analysis = await _financialRepository.GetByProjectIdAsync(request.ProjectId);

        if (analysis == null)
        {
            analysis = new ProjectFinancialAnalysis(request.ProjectId);
            analysis.Update(
                request.RevenueProjections,
                request.CostAnalysis,
                request.BreakEvenAnalysis,
                request.FundingRequirements,
                request.KeyIndicators
            );
            await _financialRepository.AddAsync(analysis);
        }
        else
        {
            analysis.Update(
                request.RevenueProjections,
                request.CostAnalysis,
                request.BreakEvenAnalysis,
                request.FundingRequirements,
                request.KeyIndicators
            );
            await _financialRepository.UpdateAsync(analysis);
        }

        return true;
    }
}

