using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Comments.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);

        if (comment is null)
            throw new NotFoundException("Yorum bulunamadı");

        var isAuthor = comment.AuthorId == request.RequesterId;

        var isOwner = false;
        if (!isAuthor)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == comment.TaskItemId, cancellationToken);

            if (task is not null)
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == task.ProjectId, cancellationToken);

                isOwner = project is not null && project.OwnerId == request.RequesterId;
            }
        }

        if (!isAuthor && !isOwner)
            throw new ForbiddenException("Bu yorumu silme yetkiniz yok");

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}