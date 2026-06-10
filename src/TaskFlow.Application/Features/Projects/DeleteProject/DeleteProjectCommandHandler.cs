using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Application.Features.Projects.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _context.Projects
           .FirstOrDefaultAsync(c => c.Id == request.ProjectId, cancellationToken);

        if (project is null)
            throw new NotFoundException("Proje bulunamadı");

        if (project.OwnerId != request.RequesterId)
            throw new ForbiddenException("Bu projeyi silme yetkiniz yok");


        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
    }
}