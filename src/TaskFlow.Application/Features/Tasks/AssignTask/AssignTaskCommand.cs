using MediatR;

namespace TaskFlow.Application.Features.Tasks.AssignTask;

public record AssignTaskCommand(
    Guid TaskId,
    Guid? AssignedToId,
    Guid RequesterId
) : IRequest;