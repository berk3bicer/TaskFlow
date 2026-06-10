using TaskFlow.Application.Features.Comments.EditComment;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class EditCommentHandlerTests
{
    [Fact]
    public async Task Handle_WhenRequesterIsAuthor_EditsContent()
    {
        await using var context = TestDbContextFactory.Create();
        var authorId = Guid.NewGuid();
        var comment = new Comment("Eski içerik", Guid.NewGuid(), authorId);
        context.Comments.Add(comment);
        await context.SaveChangesAsync();

        var handler = new EditCommentCommandHandler(context);
        var command = new EditCommentCommand(comment.Id, "Yeni içerik", authorId);

        await handler.Handle(command, CancellationToken.None);

        var updated = await context.Comments.FindAsync(comment.Id);
        Assert.Equal("Yeni içerik", updated!.Content);
    }

    [Fact]
    public async Task Handle_WhenRequesterIsNotAuthor_ThrowsForbidden()
    {
        await using var context = TestDbContextFactory.Create();
        var authorId = Guid.NewGuid();
        var strangerId = Guid.NewGuid();
        var comment = new Comment("Eski içerik", Guid.NewGuid(), authorId);
        context.Comments.Add(comment);
        await context.SaveChangesAsync();

        var handler = new EditCommentCommandHandler(context);
        var command = new EditCommentCommand(comment.Id, "Yeni içerik", strangerId);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenCommentNotFound_ThrowsNotFound()
    {
        await using var context = TestDbContextFactory.Create();
        var handler = new EditCommentCommandHandler(context);
        var command = new EditCommentCommand(Guid.NewGuid(), "İçerik", Guid.NewGuid());

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }
}