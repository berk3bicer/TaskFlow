using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Tasks.CreateTask;

public class CreateTaskCommandHandler
    : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project is null)
            throw new DomainException("Proje bulunamadı");

        if (project.OwnerId != request.RequesterId)
            throw new DomainException("Bu projeye task ekleme yetkiniz yok");

        var task = new TaskItem(
            request.Title,
            request.Description,
            request.ProjectId,
            request.DueDate);

        _context.Tasks.Add(task);

        await _context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}