using TaskFlow.Application.Features.Tasks.DeleteTask;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class DeleteTaskHandlerTests
{
    [Fact]
    public async Task Handle_WhenRequesterIsProjectOwner_DeletesTask()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = Guid.NewGuid();
        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);
        var task = new TaskItem("Task", null, project.Id);
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var handler = new DeleteTaskCommandHandler(context);
        var command = new DeleteTaskCommand(task.Id, ownerId);

        await handler.Handle(command, CancellationToken.None);

        var deleted = await context.Tasks.FindAsync(task.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Handle_WhenRequesterIsNotOwner_ThrowsForbidden()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = Guid.NewGuid();
        var strangerId = Guid.NewGuid();
        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);
        var task = new TaskItem("Task", null, project.Id);
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var handler = new DeleteTaskCommandHandler(context);
        var command = new DeleteTaskCommand(task.Id, strangerId);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenTaskNotFound_ThrowsNotFound()
    {
        await using var context = TestDbContextFactory.Create();
        var handler = new DeleteTaskCommandHandler(context);
        var command = new DeleteTaskCommand(Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }
}