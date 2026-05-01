using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Marketplace.Queries.GetMarketplace;

public class GetMarketplaceQueryHandler : IRequestHandler<GetMarketplaceQuery, IEnumerable<MarketplaceProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public GetMarketplaceQueryHandler(IProductRepository productRepository, IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<MarketplaceProductDto>> Handle(GetMarketplaceQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetMarketplaceAsync();
        var dtos = new List<MarketplaceProductDto>();

        foreach (var product in products)
        {
            // Saltear productos con datos de proyecto inconsistentes para evitar crashes
            if (product.Project == null) continue;

            var owner = await _userRepository.GetByIdAsync(product.Project.OwnerId);
            
            dtos.Add(new MarketplaceProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Category.ToString(),
                product.Price,
                product.Images,
                product.ProjectId,
                product.Project.Title ?? "Proyecto sin título",
                owner?.Name ?? "Emprendedor Desconocido",
                product.CreatedAt
            ));
        }

        return dtos;
    }
}
