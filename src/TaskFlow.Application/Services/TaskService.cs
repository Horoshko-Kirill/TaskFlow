using FluentValidation;
using TaskFlow.Application.DTO.Tasks;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings;
using TaskFlow.Application.Models.Pagination;
using DomainTask = TaskFlow.Domain.Models.Task;

namespace TaskFlow.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;

    private readonly IValidator<CreateTaskRequest> _createValidator;
    private readonly IValidator<UpdateTaskRequest> _updateValidator;
    private readonly IValidator<PageRequest> _pageValidator;

    public TaskService(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IProjectMemberRepository projectMemberRepository,
        IValidator<CreateTaskRequest> createValidator,
        IValidator<UpdateTaskRequest> updateValidator,
        IValidator<PageRequest> pageValidator)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _projectMemberRepository = projectMemberRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _pageValidator = pageValidator;
    }

    public async Task<TaskResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (task is null)
            throw new NotFoundException(
                $"Task with id '{id}' was not found.");

        return task.ToResponse();
    }

    public async Task<PagedResult<TaskResponse>> GetAllAsync(
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        await _pageValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var result = await _taskRepository.GetTasksAsync(
            request,
            cancellationToken);

        return MapPagedResult(result);
    }

    public async Task<PagedResult<TaskResponse>> GetByProjectIdAsync(
        Guid projectId,
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        await _pageValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var project = await _projectRepository.GetByIdAsync(
            projectId,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{projectId}' was not found.");

        var result = await _taskRepository.GetByProjectIdAsync(
            projectId,
            request,
            cancellationToken);

        return MapPagedResult(result);
    }

    public async Task<PagedResult<TaskResponse>> GetByAssigneeIdAsync(
        Guid assigneeId,
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        await _pageValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var user = await _userRepository.GetUserByIdAsync(
            assigneeId,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{assigneeId}' was not found.");

        var result = await _taskRepository.GetByAssigneeIdAsync(
            assigneeId,
            request,
            cancellationToken);

        return MapPagedResult(result);
    }

    public async Task<PagedResult<TaskResponse>> GetByProjectAndAssigneeAsync(
        Guid projectId,
        Guid assigneeId,
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        await _pageValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var project = await _projectRepository.GetByIdAsync(
            projectId,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{projectId}' was not found.");

        var user = await _userRepository.GetUserByIdAsync(
            assigneeId,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{assigneeId}' was not found.");

        var result = await _taskRepository.GetByProjectAndAssigneeAsync(
            projectId,
            assigneeId,
            request,
            cancellationToken);

        return MapPagedResult(result);
    }

    public async Task<TaskResponse> CreateAsync(
        CreateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var project = await _projectRepository.GetByIdAsync(
            request.ProjectId,
            cancellationToken);

        if (project is null)
            throw new NotFoundException(
                $"Project with id '{request.ProjectId}' was not found.");

        await ValidateAssigneeAsync(
            request.ProjectId,
            request.AssigneeId,
            cancellationToken);

        var task = request.ToEntity();

        await _taskRepository.AddAsync(
            task,
            cancellationToken);

        return task.ToResponse();
    }

    public async Task<TaskResponse> UpdateAsync(
        Guid id,
        UpdateTaskRequest request,
        CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var task = await _taskRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (task is null)
            throw new NotFoundException(
                $"Task with id '{id}' was not found.");

        await ValidateAssigneeAsync(
            task.ProjectId,
            request.AssigneeId,
            cancellationToken);

        request.UpdateEntity(task);

        await _taskRepository.UpdateAsync(
            task,
            cancellationToken);

        return task.ToResponse();
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (task is null)
            throw new NotFoundException(
                $"Task with id '{id}' was not found.");

        await _taskRepository.DeleteAsync(
            task,
            cancellationToken);
    }

    private async Task ValidateAssigneeAsync(
        Guid projectId,
        Guid? assigneeId,
        CancellationToken cancellationToken)
    {
        if (assigneeId is null)
            return;

        var user = await _userRepository.GetUserByIdAsync(
            assigneeId.Value,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{assigneeId}' was not found.");

        var member = await _projectMemberRepository.GetAsync(
            projectId,
            assigneeId.Value,
            cancellationToken);

        if (member is null)
            throw new BadRequestException(
                "Task assignee must be a member of the project.");
    }

    private static PagedResult<TaskResponse> MapPagedResult(
        PagedResult<DomainTask> result)
    {
        return new PagedResult<TaskResponse>
        {
            Items = result.Items
                .Select(x => x.ToResponse())
                .ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
}