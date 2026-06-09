using MediatR;

namespace TaskFlow.Application.Features.Tasks.DeleteTask;

public record DeleteTaskCommand(
    Guid TaskId,
    Guid RequesterId
) : IRequest;