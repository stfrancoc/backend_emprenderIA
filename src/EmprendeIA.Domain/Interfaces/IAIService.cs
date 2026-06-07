namespace EmprendeIA.Domain.Interfaces;

public interface IAIService
{
    Task<object> GenerateBmcAsync(object input);
    Task<object> GenerateFinancialAnalysisAsync(object input);
    Task<object> ChatAsync(object input);
    Task<ProductClassifyResponse?> ClassifyProductAsync(string name, string description);
    Task<CalculateMatchesResponse?> CalculateMatchesAsync(string projectId, string bmcText);
}