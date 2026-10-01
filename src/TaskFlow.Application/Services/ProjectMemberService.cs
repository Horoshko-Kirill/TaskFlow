using TaskFlow.Application.DTO.ProjectMembers;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Services;

public class ProjectMemberService : IProjectMemberService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserRepository _userRepository;

    public ProjectMemberService(
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository,
        IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
        _userRepository = userRepository;
    }
    
    public async Task<PagedResult<ProjectMemberListItemResponse>> GetByProjectIdAsync(Guid projectId, PageRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(
            projectId,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{projectId}' was not found.");

        var result = await _projectMemberRepository.GetByProjectIdAsync(
            projectId,
            request,
            cancellationToken);

        return new PagedResult<ProjectMemberListItemResponse>
        {
            Items = result.Items
                .Select(x => x.ToListItemResponse())
                .ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<ProjectMemberResponse> AddAsync(Guid projectId, AddProjectMemberRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(
            projectId,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{projectId}' was not found.");

        var user = await _userRepository.GetUserByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{request.UserId}' was not found.");

        var existingMember = await _projectMemberRepository.GetAsync(
            projectId,
            request.UserId,
            cancellationToken);

        if (existingMember is not null)
            throw new ConflictException(
                "User is already a member of this project.");

        var member = request.ToEntity(projectId);

        await _projectMemberRepository.AddAsync(
            member,
            cancellationToken);

        return member.ToResponse();
    }

    public async Task DeleteAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default)
    {
        var member = await _projectMemberRepository.GetAsync(
            projectId,
            userId,
            cancellationToken);

        if (member is null)
            throw new NotFoundException(
                "Project member was not found.");

        await _projectMemberRepository.DeleteAsync(
            member,
            cancellationToken);
    }
}