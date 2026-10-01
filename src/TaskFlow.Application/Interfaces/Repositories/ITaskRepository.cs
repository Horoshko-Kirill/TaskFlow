using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskFlow.Domain.Models.Task?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TaskFlow.Domain.Models.Task>> GetTasksAsync(
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TaskFlow.Domain.Models.Task>> GetByProjectIdAsync(
        Guid projectId,
        PageRequest request,
        CancellationToken cancellationToken = default);
    
    Task<PagedResult<TaskFlow.Domain.Models.Task>> GetByAssigneeIdAsync(
        Guid assigneeId,
        PageRequest request,
        CancellationToken cancellationToken = default);
    
    Task<PagedResult<TaskFlow.Domain.Models.Task>> GetByProjectAndAssigneeAsync(
        Guid projectId,
        Guid assigneeId,
        PageRequest request,
        CancellationToken cancellationToken = default);
    
    Task AddAsync(
        TaskFlow.Domain.Models.Task task,
        CancellationToken cancellationToken = default);
    
    Task UpdateAsync(
        TaskFlow.Domain.Models.Task task,
        CancellationToken cancellationToken = default);
    
    Task DeleteAsync(
        TaskFlow.Domain.Models.Task task,
        CancellationToken cancellationToken = default);
}