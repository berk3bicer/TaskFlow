using MediatR;

namespace TaskFlow.Application.Features.Comments.GetComments;

public record GetCommentsQuery(
    Guid TaskItemId,
    Guid RequesterId
) : IRequest<List<CommentDto>>;