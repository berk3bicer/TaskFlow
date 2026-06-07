namespace TaskFlow.Application.Features.Projects.GetProjects;

public record ProjectDto(
    Guid Id,
    string Name,
    string? Description
);