using MediatR;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Features.Tasks.UpdateTaskStatus;

public record UpdateTaskStatusCommand(
    Guid TaskId,
    TaskItemStatus NewStatus,
    Guid RequesterId
) : IRequest;