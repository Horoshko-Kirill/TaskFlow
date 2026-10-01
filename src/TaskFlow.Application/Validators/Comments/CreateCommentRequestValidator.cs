using FluentValidation;
using TaskFlow.Application.DTO.Comments;

namespace TaskFlow.Application.Validators.Comments;

public class CreateCommentRequestValidator
    : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty();

        RuleFor(x => x.AuthorId)
            .NotEmpty();

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(5000);
    }
}