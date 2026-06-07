using MediatR;

namespace TaskFlow.Application.Features.Tasks.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    Guid ProjectId,
    DateTime? DueDate,
    Guid RequesterId
) : IRequest<Guid>;