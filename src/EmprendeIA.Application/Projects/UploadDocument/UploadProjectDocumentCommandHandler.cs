using MediatR;
using EmprendeIA.Domain.Interfaces;

namespace EmprendeIA.Application.Projects.UploadDocument;

public class UploadProjectDocumentCommandHandler : IRequestHandler<UploadProjectDocumentCommand, string>
{
    private readonly IAIService _aiService;
    private readonly IProjectRepository _projectRepository;

    public UploadProjectDocumentCommandHandler(IAIService aiService, IProjectRepository projectRepository)
    {
        _aiService = aiService;
        _projectRepository = projectRepository;
    }

    public async Task<string> Handle(UploadProjectDocumentCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null || project.OwnerId != request.UserId)
            throw new Exception("Proyecto no encontrado o sin permisos.");

        var text = DocumentTextExtractor.ExtractText(request.FileStream, request.FileName);

        if (string.IsNullOrWhiteSpace(text))
            throw new Exception("No se pudo extraer texto del documento.");

        var docId = await _aiService.IngestRagDocumentAsync(
            text,
            source: request.FileName,
            projectId: request.ProjectId.ToString()
        );

        return docId;
    }
}
