using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;

namespace TaskFlow.Application.Features.Tasks.GetTask;

public class GetTaskQueryHandler
    : IRequestHandler<GetTaskQuery, List<TaskDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTaskQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> Handle(
        GetTaskQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Tasks
    .AsNoTracking()
    .Where(t => t.ProjectId == request.ProjectId)
    .OrderByDescending(t => t.CreatedAt)
    .Select(t => new TaskDto(
        t.Id,
        t.ProjectId,
        t.AssignedToId,
        t.Title,
        t.Description,
        t.DueDate,
        t.Status.ToString(),
        t.UpdatedAt))
    .ToListAsync(cancellationToken);
    }
}