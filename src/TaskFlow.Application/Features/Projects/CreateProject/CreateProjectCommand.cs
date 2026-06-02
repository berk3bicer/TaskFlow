using MediatR;

namespace TaskFlow.Application.Features.Projects.CreateProject;

public record CreateProjectCommand(
    string Name,
    string? Description,
    Guid OwnerId
) : IRequest<Guid>;