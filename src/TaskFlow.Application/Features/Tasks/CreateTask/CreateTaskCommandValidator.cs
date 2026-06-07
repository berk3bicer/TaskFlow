using FluentValidation;

namespace TaskFlow.Application.Features.Tasks.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Görev Başlığı boş olamaz.")
            .MaximumLength(200).WithMessage("Görev Başlığı 200 karakteri geçemez");

        RuleFor(t => t.ProjectId)
            .NotEmpty().WithMessage("Proje ID zorunludur.");

        RuleFor(t => t.Description)
            .MaximumLength(2000).WithMessage("Görev açıklaması 2000 karakteri geçemez");

        RuleFor(t => t.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(t => t.DueDate.HasValue)
            .WithMessage("Bitiş tarihi geçmişte olamaz");
    }

}