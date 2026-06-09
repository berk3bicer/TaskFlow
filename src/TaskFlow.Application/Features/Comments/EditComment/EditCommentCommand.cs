using MediatR;

namespace TaskFlow.Application.Features.Comments.EditComment;

public record EditCommentCommand(
    Guid CommentId,
    string Content,
    Guid RequesterId
) : IRequest;