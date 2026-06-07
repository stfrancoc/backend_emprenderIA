namespace EmprendeIA.Domain.Interfaces;

public record ProductClassifyResponse(
    string Category,
    double Confidence,
    string Reasoning
);
