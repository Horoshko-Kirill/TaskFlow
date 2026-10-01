using FluentValidation;
using TaskFlow.Application.DTO.Users;

namespace TaskFlow.Application.Validators.Users;

public class UpdateUserRequestValidator
    : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
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