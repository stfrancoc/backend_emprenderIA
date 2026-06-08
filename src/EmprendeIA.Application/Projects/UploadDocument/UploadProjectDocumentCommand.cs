using MediatR;

namespace EmprendeIA.Application.Projects.UploadDocument;

public record UploadProjectDocumentCommand(
    Guid ProjectId,
    Guid UserId,
    Stream FileStream,
    string FileName
) : IRequest<string>;
