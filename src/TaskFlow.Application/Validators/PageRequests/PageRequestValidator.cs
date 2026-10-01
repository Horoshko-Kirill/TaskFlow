using FluentValidation;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Validators.PageRequests;

public class PageRequestValidator
    : AbstractValidator<PageRequest>
{
    public PageRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 100);
    }
}