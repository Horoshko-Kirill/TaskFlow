using TaskFlow.Application.Models.Pagination;
using TaskFlow.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(
        string email, 
        CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetUsersAsync(
        PageRequest request, 
        CancellationToken cancellationToken = default);
    Task AddAsync(
        User user, 
        CancellationToken cancellationToken = default);
    Task UpdateAsync(
        User user, 
        CancellationToken cancellationToken = default);
    Task DeleteAsync(
        Guid id, 
        CancellationToken cancellationToken = default);
}