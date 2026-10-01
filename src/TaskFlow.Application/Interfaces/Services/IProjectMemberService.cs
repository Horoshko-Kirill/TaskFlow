using TaskFlow.Application.DTO.ProjectMembers;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Interfaces.Services;

public interface IProjectMemberService
{
    Task<PagedResult<ProjectMemberListItemResponse>> GetByProjectIdAsync(
        Guid projectId,
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<ProjectMemberResponse> AddAsync(
        Guid projectId,
        AddProjectMemberRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken = default);
}