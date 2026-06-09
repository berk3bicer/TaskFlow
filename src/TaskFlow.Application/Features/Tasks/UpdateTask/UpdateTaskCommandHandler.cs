using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;


namespace TaskFlow.Application.Features.Tasks.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            throw new NotFoundException("Task bulunamadı");

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == task.ProjectId, cancellationToken);

        if (project == null || project.OwnerId != request.RequesterId)
            throw new ForbiddenException("Bu task'ı düzenleme yetkiniz yok");

        task.UpdateDetails(request.Title, request.Description, request.DueDate);
        await _context.SaveChangesAsync(cancellationToken);
    }

}