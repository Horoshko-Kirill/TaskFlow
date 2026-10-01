using FluentValidation;
using TaskFlow.Application.DTO.Projects;

namespace TaskFlow.Application.Validators.Projects;

public class UpdateProjectRequestValidator
    : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}