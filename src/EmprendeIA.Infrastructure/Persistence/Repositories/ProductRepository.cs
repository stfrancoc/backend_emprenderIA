using EmprendeIA.Domain.Entities.Marketplace;
using EmprendeIA.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using EmprendeIA.Infrastructure.Persistence;

namespace EmprendeIA.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Metrics)
            .Include(p => p.Project)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetMarketplaceAsync()
    {
        return await _context.Products
            .Include(p => p.Project)
            .Where(p => p.Visibility)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByProjectAsync(Guid projectId)
    {
        return await _context.Products
            .Where(p => p.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddMetricsAsync(ProductMetrics metrics)
    {
        await _context.ProductMetrics.AddAsync(metrics);
        await _context.SaveChangesAsync();
    }
}
