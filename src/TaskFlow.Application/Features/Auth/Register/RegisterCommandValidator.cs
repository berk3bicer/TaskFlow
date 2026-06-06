using FluentValidation;

namespace TaskFlow.Application.Features.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email boş olmamalı.")
            .EmailAddress();

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Şifre boş olmamalı.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakterli olmalı.");

        RuleFor(r => r.FullName)
            .NotEmpty()
            .MaximumLength(150).WithMessage("Maksimum 150 karakter olmalı.");
    }
}