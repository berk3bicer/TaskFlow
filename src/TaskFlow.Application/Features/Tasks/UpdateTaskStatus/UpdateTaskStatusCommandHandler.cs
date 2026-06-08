using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Tasks.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler
    : IRequestHandler<UpdateTaskStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTaskStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateTaskStatusCommand request,
        CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task is null)
            throw new NotFoundException("Görev bulunamadı");

        if (task.Project!.OwnerId != request.RequesterId)
            throw new ForbiddenException("Bu görevi güncelleme yetkiniz yok");

        task.UpdateStatus(request.NewStatus);

        await _context.SaveChangesAsync(cancellationToken);
    }
}