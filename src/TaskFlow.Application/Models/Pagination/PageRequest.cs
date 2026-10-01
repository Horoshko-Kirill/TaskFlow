namespace TaskFlow.Application.Models.Pagination;

public class PageRequest
{
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;
}