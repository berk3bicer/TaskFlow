using MediatR;

namespace TaskFlow.Application.Features.Comments.AddComment;

public record AddCommentCommand(
    Guid RequesterId,
    Guid TaskItemId,
    string Content
) : IRequest<Guid>;