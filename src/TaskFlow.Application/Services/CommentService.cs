using TaskFlow.Application.DTO.Comments;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public CommentService(
        ICommentRepository commentRepository,
        ITaskRepository taskRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }
    
    public async Task<PagedResult<CommentResponse>> GetByTaskIdAsync(Guid taskId, PageRequest request, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(
            taskId,
            cancellationToken);

        if (task is null)
            throw new NotFoundException(
                $"Task with id '{taskId}' was not found.");

        var result = await _commentRepository.GetByTaskIdAsync(
            taskId,
            request,
            cancellationToken);

        return new PagedResult<CommentResponse>
        {
            Items = result.Items
                .Select(x => x.ToResponse())
                .ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<CommentResponse> CreateAsync(CreateCommentRequest request, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(
            request.TaskId,
            cancellationToken);

        if (task is null)
            throw new NotFoundException(
                $"Task with id '{request.TaskId}' was not found.");

        var user = await _userRepository.GetUserByIdAsync(
            request.AuthorId,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{request.AuthorId}' was not found.");

        var comment = request.ToEntity();

        await _commentRepository.AddAsync(
            comment,
            cancellationToken);

        return comment.ToResponse();
    }

    public async Task<CommentResponse> UpdateAsync(Guid id, UpdateCommentRequest request, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (comment is null)
            throw new NotFoundException(
                $"Comment with id '{id}' was not found.");

        request.UpdateEntity(comment);

        await _commentRepository.UpdateAsync(
            comment,
            cancellationToken);

        return comment.ToResponse();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _commentRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (comment is null)
            throw new NotFoundException(
                $"Comment with id '{id}' was not found.");

        await _commentRepository.DeleteAsync(
            comment,
            cancellationToken);
    }
}