using MediatR;

namespace EmprendeIA.Application.Marketplace.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id, Guid UserId) : IRequest<bool>;
