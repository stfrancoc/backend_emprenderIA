using MediatR;

namespace EmprendeIA.Application.Projects.GenerateBmc;

public record GenerateBmcCommand(Guid ProjectId, Guid UserId) : IRequest<object>;