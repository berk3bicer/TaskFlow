using FluentValidation;

namespace TaskFlow.Application.Features.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty().WithMessage("Email boş olamaz.");

        RuleFor(l => l.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz");
    }
}