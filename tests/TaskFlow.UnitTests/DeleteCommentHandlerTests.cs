using TaskFlow.Application.Features.Comments.DeleteComment;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class DeleteCommentHandlerTests
{
    // SENARYO 1: Yorumun yazarı kendi yorumunu silebilmeli
    [Fact]
    public async Task Handle_WhenRequesterIsAuthor_DeletesComment()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var ownerId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);

        var task = new TaskItem("Task", null, project.Id);
        context.Tasks.Add(task);

        var comment = new Comment("Yorum", task.Id, authorId);
        context.Comments.Add(comment);

        await context.SaveChangesAsync();

        var handler = new DeleteCommentCommandHandler(context);
        var command = new DeleteCommentCommand(comment.Id, authorId);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert — yorum gerçekten silindi mi?
        var deleted = await context.Comments.FindAsync(comment.Id);
        Assert.Null(deleted);
    }

    // SENARYO 2: Proje sahibi (yazar olmasa bile) yorumu silebilmeli (moderasyon)
    [Fact]
    public async Task Handle_WhenRequesterIsProjectOwner_DeletesComment()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var ownerId = Guid.NewGuid();
        var authorId = Guid.NewGuid();   // owner'dan farklı biri yazmış

        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);

        var task = new TaskItem("Task", null, project.Id);
        context.Tasks.Add(task);

        var comment = new Comment("Yorum", task.Id, authorId);
        context.Comments.Add(comment);

        await context.SaveChangesAsync();

        var handler = new DeleteCommentCommandHandler(context);
        // İsteği OWNER yapıyor, yazar değil
        var command = new DeleteCommentCommand(comment.Id, ownerId);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var deleted = await context.Comments.FindAsync(comment.Id);
        Assert.Null(deleted);
    }

    // SENARYO 3: Ne yazar ne owner olan biri silmeye çalışırsa ForbiddenException
    [Fact]
    public async Task Handle_WhenRequesterIsStranger_ThrowsForbidden()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var ownerId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var strangerId = Guid.NewGuid();   // ne owner ne yazar

        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);

        var task = new TaskItem("Task", null, project.Id);
        context.Tasks.Add(task);

        var comment = new Comment("Yorum", task.Id, authorId);
        context.Comments.Add(comment);

        await context.SaveChangesAsync();

        var handler = new DeleteCommentCommandHandler(context);
        var command = new DeleteCommentCommand(comment.Id, strangerId);

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }

    // SENARYO 4: Olmayan bir yorum silinmeye çalışılırsa NotFoundException
    [Fact]
    public async Task Handle_WhenCommentNotFound_ThrowsNotFound()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var handler = new DeleteCommentCommandHandler(context);
        var command = new DeleteCommentCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }
}