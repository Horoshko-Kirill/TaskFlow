using FluentValidation;
using TaskFlow.Application.DTO.ProjectMembers;

namespace TaskFlow.Application.Validators.ProjectMembers;

public class AddProjectMemberRequestValidator
    : AbstractValidator<AddProjectMemberRequest>
{
    public AddProjectMemberRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}