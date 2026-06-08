using FluentValidation;

namespace TaskFlow.Application.Features.Comments.AddComment;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(c => c.Content)
            .NotEmpty().WithMessage("Yorum boş olamaz")
            .MaximumLength(1000);

        RuleFor(c => c.TaskItemId)
            .NotEmpty().WithMessage("TaskId zorunludur.");
    }
}