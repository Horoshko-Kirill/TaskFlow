using FluentValidation;
using TaskFlow.Application.DTO.Projects;

namespace TaskFlow.Application.Validators.Projects;

public class CreateProjectRequestValidator
    : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}