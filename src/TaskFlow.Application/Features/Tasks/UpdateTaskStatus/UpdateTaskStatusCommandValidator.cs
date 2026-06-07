using FluentValidation;

namespace TaskFlow.Application.Features.Tasks.UpdateTaskStatus;

public class UpdateTaskStatusCommandValidator
    : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("Task ID zorunludur.");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Geçersiz görev durumu.");
    }
}