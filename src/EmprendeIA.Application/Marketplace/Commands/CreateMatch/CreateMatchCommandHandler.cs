using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EmprendeIA.Domain.Projects;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Marketplace.Commands.CreateMatch;

public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Guid?>
{
    private readonly IProjectMatchRepository _matchRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateMatchCommandHandler(IProjectMatchRepository matchRepository, IProjectRepository projectRepository)
    {
        _matchRepository = matchRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Guid?> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
    {
        // Verificar que el proyecto exista
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null) return null;

        // Validar si ya existe el match
        var existingMatch = await _matchRepository.GetMatchAsync(request.ProjectId, request.UserId);
        if (existingMatch != null) return existingMatch.Id;

        // Por ahora simularemos un score básico entre 50 y 100 hasta integrar la llamada HTTP al motor de IA
        var random = new Random();
        decimal fakeScore = (decimal)(random.NextDouble() * 50 + 50);

        var match = new EmprendeIA.Domain.Projects.ProjectMatch(request.ProjectId, request.UserId, fakeScore);
        
        await _matchRepository.AddAsync(match);

        return match.Id;
    }
}
