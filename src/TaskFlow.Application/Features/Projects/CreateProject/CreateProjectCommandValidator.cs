using FluentValidation;

namespace TaskFlow.Application.Features.Projects.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Proje Adı Zorunludur.")
            .MaximumLength(100).WithMessage("Proje adı 100 karakteri geçemez.");

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.OwnerId)
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir sahip belirtilmelidir.");
    }
}