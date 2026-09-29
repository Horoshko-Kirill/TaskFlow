namespace TaskFlow.Domain.Models;


public class Comment
{
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }

    public Guid AuthorId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Task Task { get; set; } = null!;

    public User Author { get; set; } = null!;
}