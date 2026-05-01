using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Marketplace.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IProjectRepository _projectRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository, IProjectRepository projectRepository)
    {
        _productRepository = productRepository;
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null) return false;

        var project = await _projectRepository.GetByIdAsync(product.ProjectId);
        if (project == null || project.OwnerId != request.UserId) return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }
}
