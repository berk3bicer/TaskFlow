using MediatR;

namespace TaskFlow.Application.Features.Tasks.UpdateTask;

public record UpdateTaskCommand(
    Guid TaskId,
    Guid RequesterId,
    string Title,
    string? Description,
    DateTime? DueDate
) : IRequest;