using TaskFlow.Application.DTO.Tasks;
using DomainTask = TaskFlow.Domain.Models.Task;

namespace TaskFlow.Application.Mappings;

public static class TaskMapping
{
    public static DomainTask ToEntity(this CreateTaskRequest request)
    {
        return new DomainTask
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            AssigneeId = request.AssigneeId,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            Priority = request.Priority,
            CreatedAt = DateTime.UtcNow,
            DueDate = request.DueDate
        };
    }

    public static void UpdateEntity(
        this UpdateTaskRequest request,
        DomainTask task)
    {
        task.AssigneeId = request.AssigneeId;
        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
    }

    public static TaskResponse ToResponse(
        this DomainTask task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            AssigneeId = task.AssigneeId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate
        };
    }
}