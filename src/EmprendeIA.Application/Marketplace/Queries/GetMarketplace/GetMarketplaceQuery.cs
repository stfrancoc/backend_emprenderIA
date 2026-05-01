using MediatR;

namespace EmprendeIA.Application.Marketplace.Queries.GetMarketplace;

public record GetMarketplaceQuery() : IRequest<IEnumerable<MarketplaceProductDto>>;
