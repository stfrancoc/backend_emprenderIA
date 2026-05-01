using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Marketplace.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository, IProjectRepository projectRepository)
    {
        _productRepository = productRepository;
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null) return false;

        // Check project ownership
        var project = await _projectRepository.GetByIdAsync(product.ProjectId);
        if (project == null || project.OwnerId != request.UserId) return false;

        product.Update(
            request.Name,
            request.Description,
            request.Category,
            request.Price,
            request.Images,
            request.Visibility
        );

        await _productRepository.UpdateAsync(product);
        return true;
    }
}
