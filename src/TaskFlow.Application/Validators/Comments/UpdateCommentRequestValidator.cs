using FluentValidation;
using TaskFlow.Application.DTO.Comments;

namespace TaskFlow.Application.Validators.Comments;

public class UpdateCommentRequestValidator
    : AbstractValidator<UpdateCommentRequest>
{
    public UpdateCommentRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(5000);
    }
}