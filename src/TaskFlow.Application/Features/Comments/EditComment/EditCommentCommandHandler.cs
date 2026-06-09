using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Comments.EditComment;

public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand>
{
    private readonly IApplicationDbContext _context;

    public EditCommentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        EditCommentCommand request,
        CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);

        if (comment is null)
            throw new NotFoundException("Yorum bulunamadı");


        if (comment.AuthorId != request.RequesterId)
            throw new ForbiddenException("Bu yorumu düzenleme yetkiniz yok");


        comment.Edit(request.Content);
        await _context.SaveChangesAsync(cancellationToken);
    }
}