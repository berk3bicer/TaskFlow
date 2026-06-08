using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Comments.AddComment;

public class AddCommentCommandHandler
    : IRequestHandler<AddCommentCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public AddCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        AddCommentCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskItemId, cancellationToken);

        if (task is null)
            throw new DomainException("Task bulunamadı");

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == task.ProjectId, cancellationToken);

        if (project is null)
            throw new DomainException("Proje bulunamadı");

        var isOwner = project.OwnerId == request.RequesterId;
        var isAssignee = task.AssignedToId == request.RequesterId;

        if (!isOwner && !isAssignee)
            throw new DomainException("Bu task'a yorum yapma yetkiniz yok");

        var comment = new Comment(request.Content, request.TaskItemId, request.RequesterId);
        _context.Comments.Add(comment);
        task.AddComment(comment);

        await _context.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }
}