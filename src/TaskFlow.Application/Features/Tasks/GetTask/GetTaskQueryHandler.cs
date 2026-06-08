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
        // 1. Projeyi çek (yetki için)
        var project = await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project is null)
            throw new DomainException("Proje bulunamadı");

        var isOwner = project.OwnerId == request.RequesterId;

        // 2. Bu projede requester'a atanmış task var mı?
        var hasAssignedTask = await _context.Tasks
            .AsNoTracking()
            .AnyAsync(t => t.ProjectId == request.ProjectId
                        && t.AssignedToId == request.RequesterId,
                      cancellationToken);

        // 3. Ne owner ne de atanmış biriyse -> yetkisiz
        if (!isOwner && !hasAssignedTask)
            throw new DomainException("Bu projenin görevlerini görme yetkiniz yok");

        // 4. Task sorgusu — owner hepsini, assignee sadece kendininkini görür
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