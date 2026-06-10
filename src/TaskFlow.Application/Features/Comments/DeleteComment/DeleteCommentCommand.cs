using MediatR;

namespace TaskFlow.Application.Features.Comments.DeleteComment;

public record DeleteCommentCommand(
    Guid CommentId,
    Guid RequesterId
) : IRequest;