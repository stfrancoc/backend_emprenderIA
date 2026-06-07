using MediatR;
using System;

namespace EmprendeIA.Application.Marketplace.Commands.CreateMatch;

public record CreateMatchCommand(Guid ProjectId, Guid UserId) : IRequest<Guid?>;
