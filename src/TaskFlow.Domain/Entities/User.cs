using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    private User() { }

    public User(string email, string passwordHash, string fullName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email boş olamaz");

        if (!email.Contains('@'))
            throw new DomainException("Geçersiz email formatı");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Şifre hash'i boş olamaz");

        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("İsim boş olamaz");

        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        FullName = fullName;
    }

    public void UpdateProfile(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("İsim boş olamaz");

        FullName = fullName;
        SetUpdatedAt();
    }

}