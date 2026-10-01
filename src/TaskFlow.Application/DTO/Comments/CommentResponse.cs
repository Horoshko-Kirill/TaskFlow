namespace TaskFlow.Application.DTO.Comments;

public class CommentResponse
{
    public Guid Id { get; init; }
    public Guid TaskId { get; init; }
    public Guid AuthorId { get; init; }
    public string Text { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}