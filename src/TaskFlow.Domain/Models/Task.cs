using TaskFlow.Domain.Enum;
using TaskStatus = TaskFlow.Domain.Enum.TaskStatus;

namespace TaskFlow.Domain.Models;

public class Task
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? AssigneeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TaskStatus Status { get; set; }

    public TaskPriority Priority { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public Project Project { get; set; } = null!;

    public User? Assignee { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}