using MediatR;

namespace TaskFlow.Application.Features.Auth.Register;

public record RegisterCommand
(
    string Email,
    string Password,
    string FullName

) : IRequest<Guid>;