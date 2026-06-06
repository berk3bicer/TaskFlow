using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Auth.Login;
using TaskFlow.Application.Features.Auth.Register;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]

    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }
}