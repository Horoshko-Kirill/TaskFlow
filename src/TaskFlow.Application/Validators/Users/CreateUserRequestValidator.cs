using FluentValidation;
using TaskFlow.Application.DTO.Users;

namespace TaskFlow.Application.Validators.Users;

public class CreateUserRequestValidator 
    : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);
    }
}