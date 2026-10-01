using TaskFlow.Application.DTO.Users;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<UserResponse>> GetAllAsync(
        PageRequest request,
        CancellationToken cancellationToken = default);

    Task<UserResponse> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default);

    Task<UserResponse> UpdateAsync(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}