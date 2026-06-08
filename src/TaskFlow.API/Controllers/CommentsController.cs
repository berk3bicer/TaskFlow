using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Features.Comments.AddComment;
using TaskFlow.Application.Features.Comments.GetComments;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/tasks/{taskId}/comments")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(
        Guid taskId,
        AddCommentCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var commandToSend = command with
        {
            TaskItemId = taskId,
            RequesterId = userId
        };

        var commentId = await _mediator.Send(commandToSend);
        return Ok(commentId);
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(Guid taskId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetCommentsQuery(taskId, userId);
        var comments = await _mediator.Send(query);
        return Ok(comments);
    }
}