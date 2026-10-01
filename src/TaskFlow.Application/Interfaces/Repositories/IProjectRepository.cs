using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task<PagedResult<Project>> GetProjectsAsync(
        PageRequest request, 
        CancellationToken cancellationToken = default);
    Task<PagedResult<Project>> GetByUserIdAsync(
        Guid userId, PageRequest request, 
        CancellationToken cancellationToken = default);
    Task AddAsync(
        Project project, 
        CancellationToken cancellationToken = default);
    Task UpdateAsync(
        Project project, 
        CancellationToken cancellationToken = default);
    Task DeleteAsync(
        Project project, 
        CancellationToken cancellationToken = default);
}