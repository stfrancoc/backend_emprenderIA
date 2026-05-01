namespace EmprendeIA.Domain.Entities.Marketplace;

public class ProductMetrics
{
    public Guid ProductId { get; private set; }
    public int Views { get; private set; }
    public int Clicks { get; private set; }
    public int Conversions { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation
    public Product Product { get; private set; } = null!;

    private ProductMetrics() { }

    public ProductMetrics(Guid productId)
    {
        ProductId = productId;
        Views = 0;
        Clicks = 0;
        Conversions = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegisterView()
    {
        Views++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegisterClick()
    {
        Clicks++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegisterConversion()
    {
        Conversions++;
        UpdatedAt = DateTime.UtcNow;
    }
}
