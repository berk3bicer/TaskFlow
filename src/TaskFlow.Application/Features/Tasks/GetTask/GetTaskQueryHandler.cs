using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;

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
        var project = await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project is null)
            throw new NotFoundException("Proje bulunamadı");

        var isOwner = project.OwnerId == request.RequesterId;

        var hasAssignedTask = await _context.Tasks
            .AsNoTracking()
            .AnyAsync(t => t.ProjectId == request.ProjectId
                        && t.AssignedToId == request.RequesterId,
                      cancellationToken);

        if (!isOwner && !hasAssignedTask)
            throw new ForbiddenException("Bu projenin görevlerini görme yetkiniz yok");

        var query = _context.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == request.ProjectId);

        if (!isOwner)
            query = query.Where(t => t.AssignedToId == request.RequesterId);

        return await query
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