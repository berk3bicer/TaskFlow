using MediatR;

namespace TaskFlow.Application.Features.Projects.GetProjects;

public record GetProjectQuery(
    Guid OwnerId
) : IRequest<List<ProjectDto>>;