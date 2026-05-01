using MediatR;
using EmprendeIA.Domain.Entities.Marketplace;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Marketplace.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid?>
{
    private readonly IProductRepository _productRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateProductCommandHandler(IProductRepository productRepository, IProjectRepository projectRepository)
    {
        _productRepository = productRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Guid?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validate Project ownership
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.UserId)
        {
            return null;
        }

        var product = new Product(
            request.ProjectId,
            request.Name,
            request.Description,
            request.Category,
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
