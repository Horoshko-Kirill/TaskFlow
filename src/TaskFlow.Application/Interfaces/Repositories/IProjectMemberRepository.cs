using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IProjectMemberRepository
{
    Task<ProjectMember?> GetAsync(
        Guid projectId, 
        Guid userId, 
        CancellationToken cancellationToken = default);
    Task<PagedResult<ProjectMember>> GetByProjectIdAsync(
        Guid projectId, 
        PageRequest request, 
        CancellationToken cancellationToken = default); 
    Task<PagedResult<ProjectMember>> GetByUserIdAsync(
        Guid userId, 
        PageRequest request, 
        CancellationToken cancellationToken = default);
    
    Task AddAsync(ProjectMember project,
        CancellationToken cancellationToken = default);
    
    Task DeleteAsync(ProjectMember project,
        CancellationToken cancellationToken = default);

}