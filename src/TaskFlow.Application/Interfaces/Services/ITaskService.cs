using TaskFlow.Application.DTO.Tasks;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Interfaces.Services;

public interface ITaskService
{
    Task<TaskResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TaskResponse>> GetAllAsync(
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TaskResponse>> GetByProjectIdAsync(
        Guid projectId,
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TaskResponse>> GetByAssigneeIdAsync(
        Guid assigneeId,
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TaskResponse>> GetByProjectAndAssigneeAsync(
        Guid projectId,
        Guid assigneeId,
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<TaskResponse> CreateAsync(
        CreateTaskRequest request,
        CancellationToken cancellationToken = default);

    Task<TaskResponse> UpdateAsync(
        Guid id,
        UpdateTaskRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}