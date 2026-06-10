using TaskFlow.Application.Features.Projects.DeleteProject;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class DeleteProjectHandlerTests
{
    [Fact]
    public async Task Handle_WhenRequesterIsOwner_DeletesProject()
    {
        await using var context = TestDbContextFactory.Create();
        var ownerId = Guid.NewGuid();
        var project = new Project("Proje", null, ownerId);
        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var handler = new DeleteProjectCommandHandler(context);
        var command = new DeleteProjectCommand(project.Id, ownerId);

        await handler.Handle(command, CancellationToken.None);

        var deleted = await context.Projects.FindAsync(project.Id);
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
        await context.SaveChangesAsync();

        var handler = new DeleteProjectCommandHandler(context);
        var command = new DeleteProjectCommand(project.Id, strangerId);

        await Assert.ThrowsAsync<ForbiddenException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WhenProjectNotFound_ThrowsNotFound()
    {
        await using var context = TestDbContextFactory.Create();
        var handler = new DeleteProjectCommandHandler(context);
        var command = new DeleteProjectCommand(Guid.NewGuid(), Guid.NewGuid());

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command, CancellationToken.None)
        );
    }
}