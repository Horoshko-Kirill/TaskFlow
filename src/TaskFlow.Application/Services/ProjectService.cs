using TaskFlow.Application.DTO.Projects;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings;
using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Application.Services;

public class ProjectService : IProjectService
{
    
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IUserRepository _userRepository;

    public ProjectService(
        IProjectRepository projectRepository,
        IProjectMemberRepository projectMemberRepository,
        IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _projectMemberRepository = projectMemberRepository;
        _userRepository = userRepository;
    }
    
    public async Task<ProjectResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{id}' was not found.");

        return project.ToResponse();
    }

    public async Task<PagedResult<ProjectResponse>> GetAllAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _projectRepository.GetProjectsAsync(
            request,
            cancellationToken);

        return new PagedResult<ProjectResponse>
        {
            Items = result.Items
                .Select(x => x.ToResponse())
                .ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<PagedResult<ProjectResponse>> GetByUserIdAsync(Guid userId, PageRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByIdAsync(
            userId,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{userId}' was not found.");

        var result = await _projectRepository.GetByUserIdAsync(
            userId,
            request,
            cancellationToken);

        return new PagedResult<ProjectResponse>
        {
            Items = result.Items
                .Select(x => x.ToResponse())
                .ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<ProjectResponse> CreateAsync(Guid userId, CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByIdAsync(
            userId,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{userId}' was not found.");

        var project = request.ToEntity();

        var member = new ProjectMember
        {
            ProjectId = project.Id,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(
            project,
            cancellationToken);

        await _projectMemberRepository.AddAsync(
            member,
            cancellationToken);

        return project.ToResponse();
    }

    public async Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{id}' was not found.");

        request.UpdateEntity(project);

        await _projectRepository.UpdateAsync(
            project,
            cancellationToken);

        return project.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{id}' was not found.");

        await _projectRepository.DeleteAsync(
            project,
            cancellationToken);
    }
}