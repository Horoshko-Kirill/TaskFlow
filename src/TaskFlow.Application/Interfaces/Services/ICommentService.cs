using TaskFlow.Application.DTO.Comments;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Interfaces.Services;

public interface ICommentService
{
    Task<PagedResult<CommentResponse>> GetByTaskIdAsync(
        Guid taskId,
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<CommentResponse> CreateAsync(
        CreateCommentRequest request,
        CancellationToken cancellationToken = default);

    Task<CommentResponse> UpdateAsync(
        Guid id,
        UpdateCommentRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}