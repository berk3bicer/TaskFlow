using MediatR;

namespace TaskFlow.Application.Features.Projects.DeleteProject;

public record DeleteProjectCommand(
    Guid ProjectId,
    Guid RequesterId
) : IRequest;