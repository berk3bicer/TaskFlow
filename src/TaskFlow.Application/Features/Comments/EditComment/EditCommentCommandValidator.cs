using FluentValidation;

namespace TaskFlow.Application.Features.Comments.EditComment;

public class EditCommentCommandValidator : AbstractValidator<EditCommentCommand>
{
    public EditCommentCommandValidator()
    {
        RuleFor(ec => ec.CommentId)
            .NotEmpty().WithMessage("Comment Id boş olamaz");

        RuleFor(ec => ec.Content)
            .NotEmpty().WithMessage("Content boş olamaz")
            .MaximumLength(1000);

    }
}