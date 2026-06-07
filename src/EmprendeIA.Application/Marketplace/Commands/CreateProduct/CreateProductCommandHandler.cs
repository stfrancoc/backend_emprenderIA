using MediatR;
using EmprendeIA.Domain.Entities.Marketplace;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Marketplace.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid?>
{
    private readonly IProductRepository _productRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IAIService _aiService;

    public CreateProductCommandHandler(IProductRepository productRepository, IProjectRepository projectRepository, IAIService aiService)
    {
        _productRepository = productRepository;
        _projectRepository = projectRepository;
        _aiService = aiService;
    }

    public async Task<Guid?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validate Project ownership
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.UserId)
        {
            return null;
        }

        var category = request.Category;

        if (category == ProductCategory.Otro)
        {
            try
            {
                var aiClassification = await _aiService.ClassifyProductAsync(request.Name, request.Description);
                if (aiClassification != null)
                {
                    var cleanCategory = aiClassification.Category.ToLowerInvariant().Replace("í", "i").Trim();
                    category = cleanCategory switch
                    {
                        "servicio" => ProductCategory.Servicio,
                        "consultoria" => ProductCategory.Consultoria,
                        "digital" => ProductCategory.Digital,
                        _ => ProductCategory.Otro
                    };
                }
            }
            catch
            {
                // Fallback silently to ProductCategory.Otro if AI service fails
                category = ProductCategory.Otro;
            }
        }

        var product = new Product(
            request.ProjectId,
            request.Name,
            request.Description,
            category,
            request.Price,
            request.Images
        );

        await _productRepository.AddAsync(product);

        // Initialize Metrics
        var metrics = new ProductMetrics(product.Id);
        await _productRepository.AddMetricsAsync(metrics);

        return product.Id;
    }
}
