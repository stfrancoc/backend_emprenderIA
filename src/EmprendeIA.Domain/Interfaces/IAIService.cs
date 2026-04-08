namespace EmprendeIA.Domain.Interfaces;

public interface IAIService
{
    Task<object> GenerateBmcAsync(object input);
}