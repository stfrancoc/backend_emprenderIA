using MediatR;
using EmprendeIA.Application.Projects.GetByUser;

namespace EmprendeIA.Application.Projects.GetById;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto?>;