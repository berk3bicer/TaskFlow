using MediatR;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Application.Features.Auth.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

        if (emailExists)
        {
            throw new ConflictException("Bu email zaten kayıtlı");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(request.Email, passwordHash, request.FullName);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}