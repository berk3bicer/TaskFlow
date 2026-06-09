using FluentValidation;

namespace TaskFlow.Application.Features.Tasks.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(ut => ut.Title)
            .NotEmpty().WithMessage("Görev Başlığı boş olamaz.")
            .MaximumLength(200).WithMessage("Görev Başlığı 200 karakteri geçemez");

        RuleFor(ut => ut.Description)
            .MaximumLength(2000).WithMessage("Görev açıklaması 2000 karakteri geçemez");

        RuleFor(ut => ut.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(ut => ut.DueDate.HasValue)
            .WithMessage("Bitiş tarihi geçmişte olamaz");
    }
}