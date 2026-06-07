using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;

namespace TaskFlow.Application.Features.Projects.GetProjects;

public class GetProjectsQueryHandler
    : IRequestHandler<GetProjectQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectDto>> Handle(
        GetProjectQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.OwnerId == request.OwnerId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto(
                p.Id,
                p.Name,
                p.Description))
            .ToListAsync(cancellationToken);
    }
}