namespace TaskFlow.Application.Features.Tasks.GetTask;

public record TaskDto(
    Guid Id,
    Guid ProjectId,
    Guid? AssignedToId,
    string Title,
    string? Description,
    DateTime? DueDate,
    string Status,
    DateTime? UpdatedAt
);