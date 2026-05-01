namespace EmprendeIA.Application.Marketplace.Queries.GetMarketplace;

public record MarketplaceProductDto(
    Guid Id,
    string Name,
    string Description,
    string Category,
    decimal Price,
    List<string> Images,
    Guid ProjectId,
    string ProjectName,
    string EntrepreneurName,
    DateTime CreatedAt
);
