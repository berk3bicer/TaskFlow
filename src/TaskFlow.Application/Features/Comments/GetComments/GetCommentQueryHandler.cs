using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;

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