using MediatR;

namespace EmprendeIA.Application.Projects.Delete;

public record DeleteProjectCommand(Guid Id, Guid OwnerId) : IRequest<bool>;