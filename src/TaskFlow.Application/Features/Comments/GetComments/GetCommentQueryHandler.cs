using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Comments.GetComments;

public class GetCommentsQueryHandler
    : IRequestHandler<GetCommentsQuery, List<CommentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCommentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CommentDto>> Handle(
        GetCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.TaskItemId, cancellationToken);

        if (task is null)
            throw new NotFoundException("Task bulunamadı");

        var project = await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == task.ProjectId, cancellationToken);

        if (project is null)
            throw new NotFoundException("Proje bulunamadı");

        var isOwner = project.OwnerId == request.RequesterId;
        var isAssignee = task.AssignedToId == request.RequesterId;

        if (!isOwner && !isAssignee)
            throw new ForbiddenException("Bu task'ın yorumlarını görme yetkiniz yok");

        return await _context.Comments
            .AsNoTracking()
            .Where(c => c.TaskItemId == request.TaskItemId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentDto(
                c.Id,
                c.TaskItemId,
                c.Content,
                c.AuthorId,
                c.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}