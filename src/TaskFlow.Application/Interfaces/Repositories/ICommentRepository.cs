using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<Comment> AddAsync(
        Comment comment,
        CancellationToken cancellationToken = default);
    
    Task DeleteAsync(
        Comment comment,
        CancellationToken cancellationToken = default);
    
    Task UpdateAsync(
        Comment comment,
        CancellationToken cancellationToken = default);

    Task<PagedResult<Comment>> GetByTaskIdAsync(
        Guid taskId,
        PageRequest request,
        CancellationToken cancellationToken = default);
}