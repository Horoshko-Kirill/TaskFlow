namespace TaskFlow.Application.DTO.Comments;

public class CreateCommentRequest
{
    public Guid TaskId { get; init; }
    public Guid AuthorId { get; init; }
    public string Text { get; init; } = null!;
}