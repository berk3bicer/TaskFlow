using MediatR;

namespace TaskFlow.Application.Features.Tasks.GetTask;

public record GetTaskQuery(
    Guid ProjectId
) : IRequest<List<TaskDto>>;
