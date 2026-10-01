using FluentValidation;
using TaskFlow.Application.DTO.Tasks;

namespace TaskFlow.Application.Validators.Tasks;

public class UpdateTaskRequestValidator
    : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(5000);

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}