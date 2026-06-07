using FluentValidation;

namespace EmprendeIA.Application.Projects.Create;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(v => v.What)
            .NotEmpty().WithMessage("What field is required.")
            .MaximumLength(280).WithMessage("What field must not exceed 280 characters.");

        RuleFor(v => v.How)
            .NotEmpty().WithMessage("How field is required.")
            .MaximumLength(280).WithMessage("How field must not exceed 280 characters.");

        RuleFor(v => v.Why)
            .NotEmpty().WithMessage("Why field is required.")
            .MaximumLength(280).WithMessage("Why field must not exceed 280 characters.");

        RuleFor(v => v.ProjectType)
            .IsInEnum().WithMessage("Invalid ProjectType.");

        RuleFor(v => v.BusinessModelType)
            .IsInEnum().WithMessage("Invalid BusinessModelType.");
    }
}
