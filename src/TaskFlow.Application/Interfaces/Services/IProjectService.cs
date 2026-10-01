using TaskFlow.Application.DTO.Projects;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ProjectResponse>> GetAllAsync(
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ProjectResponse>> GetByUserIdAsync(
        Guid userId,
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<ProjectResponse> CreateAsync(
        Guid userId,
        CreateProjectRequest request,
        CancellationToken cancellationToken = default);

    Task<ProjectResponse> UpdateAsync(
        Guid id,
        UpdateProjectRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}