using MediatR;

namespace TaskFlow.Application.Features.Tasks.GetTask;

public record GetTaskQuery(
    Guid ProjectId,
    Guid RequesterId
) : IRequest<List<TaskDto>>;
