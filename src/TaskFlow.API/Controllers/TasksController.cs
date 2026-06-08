using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Features.Tasks.CreateTask;
using TaskFlow.Application.Features.Tasks.GetTask;
using TaskFlow.Application.Features.Tasks.UpdateTaskStatus;
using TaskFlow.Application.Features.Tasks.AssignTask;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var commandWithRequester = command with { RequesterId = userId };

        var taskId = await _mediator.Send(commandWithRequester);
        return Ok(taskId);
    }


    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetByProject(Guid projectId)
    {
        var query = new GetTaskQuery(projectId);
        var tasks = await _mediator.Send(query);
        return Ok(tasks);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateStatus(
    Guid taskId,
    UpdateTaskStatusCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var commandToSend = command with
        {
            TaskId = taskId,
            RequesterId = userId
        };

        await _mediator.Send(commandToSend);
        return NoContent();
    }

    [HttpPut("{taskId}/assign")]

    public async Task<IActionResult> Assign
    (Guid taskId,
     AssignTaskCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var commandToSend = command with { TaskId = taskId, RequesterId = userId };

        await _mediator.Send(commandToSend);
        return NoContent();
    }
}