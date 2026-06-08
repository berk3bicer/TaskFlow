namespace TaskFlow.Application.Features.Comments.GetComments;

public record CommentDto(
    Guid Id,
    Guid TaskItemId,
    string Content,
    Guid AuthorId,
    DateTime CreatedAt
);