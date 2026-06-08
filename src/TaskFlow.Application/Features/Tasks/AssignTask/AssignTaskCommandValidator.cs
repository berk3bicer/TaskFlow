using FluentValidation;

namespace TaskFlow.Application.Features.Tasks.AssignTask;

public class AssignTaskCommandValidator : AbstractValidator<AssignTaskCommand>
{
    public AssignTaskCommandValidator()
    {
        RuleFor(a => a.TaskId)
            .NotEmpty().WithMessage("TaskId zorunludur.");
    }
}