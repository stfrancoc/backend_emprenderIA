namespace EmprendeIA.Domain.Interfaces;

public interface IAIService
{
    Task<object> GenerateBmcAsync(object input);
    Task<object> GenerateFinancialAnalysisAsync(object input);
    Task<object> ChatAsync(object input);
}