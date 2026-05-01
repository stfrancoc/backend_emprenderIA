using EmprendeIA.Domain.Entities.Marketplace;

namespace EmprendeIA.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetMarketplaceAsync();
    Task<IEnumerable<Product>> GetByProjectAsync(Guid projectId);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task AddMetricsAsync(ProductMetrics metrics);
}
