using TaskFlow.Application.Features.Comments.GetComments;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class GetCommentsHandlerTests
{
    [Fact]
    public async Task Handle_WhenRequesterIsOwner_ReturnsComments()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = Guid.NewGuid();
        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);
        var task = new TaskItem("Task", null, project.Id);
        context.Tasks.Add(task);
        context.Comments.Add(new Comment("Yorum 1", task.Id, Guid.NewGuid()));
        context.Comments.Add(new Comment("Yorum 2", task.Id, Guid.NewGuid()));
        await context.SaveChangesAsync();

        var handler = new GetCommentsQueryHandler(context);
        var query = new GetCommentsQuery(task.Id, ownerId);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Handle_WhenRequesterIsAssignee_ReturnsComments()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = Guid.NewGuid();
        var assigneeId = Guid.NewGuid();
        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);
        var task = new TaskItem("Task", null, project.Id);
        task.AssignTo(assigneeId);
        context.Tasks.Add(task);
        context.Comments.Add(new Comment("Yorum", task.Id, Guid.NewGuid()));
        await context.SaveChangesAsync();

        var handler = new GetCommentsQueryHandler(context);
        var query = new GetCommentsQuery(task.Id, assigneeId);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task Handle_WhenRequesterIsStranger_ThrowsForbidden()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = Guid.NewGuid();
        var strangerId = Guid.NewGuid();
        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);
        var task = new TaskItem("Task", null, project.Id);
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var handler = new GetCommentsQueryHandler(context);
        var query = new GetCommentsQuery(task.Id, strangerId);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => handler.Handle(query, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenTaskNotFound_ThrowsNotFound()
    {
        await using var context = TestDbContextFactory.Create();
        var handler = new GetCommentsQueryHandler(context);
        var query = new GetCommentsQuery(Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(query, CancellationToken.None)
        );
    }
}