using TaskFlow.Domain.Enum;
using TaskStatus = TaskFlow.Domain.Enum.TaskStatus;

namespace TaskFlow.Application.DTO.Tasks;

public class CreateTaskRequest
{
    public Guid ProjectId { get; init; }
    public Guid? AssigneeId { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public DateTime? DueDate { get; init; }
}