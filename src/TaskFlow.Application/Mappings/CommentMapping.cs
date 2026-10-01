using TaskFlow.Application.DTO.Comments;
using TaskFlow.Domain.Models;

namespace TaskFlow.Application.Mappings;

public static class CommentMapping
{
    public static Comment ToEntity(this CreateCommentRequest request)
    {
        return new Comment
        {
            Id = Guid.NewGuid(),
            TaskId = request.TaskId,
            AuthorId = request.AuthorId,
            Text = request.Text,
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public static void UpdateEntity(
        this UpdateCommentRequest request,
        Comment comment)
    {
        comment.Text = request.Text;
    }

    public static CommentResponse ToResponse(
        this Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            TaskId = comment.TaskId,
            AuthorId = comment.AuthorId,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt
        };
    }
}