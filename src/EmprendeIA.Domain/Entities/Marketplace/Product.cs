using EmprendeIA.Domain.Projects;

namespace EmprendeIA.Domain.Entities.Marketplace;

public class Product
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProductCategory Category { get; private set; }
    public decimal Price { get; private set; }
    public List<string> Images { get; private set; } = new();
    public bool Visibility { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation
    public Project Project { get; private set; } = null!;
    public ProductMetrics Metrics { get; private set; } = null!;

    private Product() { }

    public Product(Guid projectId, string name, string description, ProductCategory category, decimal price, List<string>? images = null)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Name = name;
        Description = description;
        Category = category;
        Price = price;
        Images = images ?? new List<string>();
        Visibility = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, ProductCategory category, decimal price, List<string> images, bool visibility)
    {
        Name = name;
        Description = description;
        Category = category;
        Price = price;
        Images = images;
        Visibility = visibility;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetVisibility(bool visibility)
    {
        Visibility = visibility;
        UpdatedAt = DateTime.UtcNow;
    }
}
