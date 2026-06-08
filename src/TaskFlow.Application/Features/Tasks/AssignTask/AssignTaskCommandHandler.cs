using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Tasks.AssignTask;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public AssignTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        AssignTaskCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            throw new DomainException("Task bulunamadı");

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == task.ProjectId, cancellationToken);

        if (project is null)
            throw new DomainException("Proje bulunamadı");

        if (project.OwnerId != request.RequesterId)
            throw new DomainException("Bu task'ı atama yetkiniz yok");

        if (request.AssignedToId is not null)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.AssignedToId, cancellationToken);

            if (!userExists)
                throw new DomainException("Atanacak kullanıcı bulunamadı");

            task.AssignTo(request.AssignedToId.Value);
        }
        else
        {
            task.Unassign();
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}