using FluentValidation;
using TaskFlow.Application.DTO.Users;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Mappings;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IValidator<CreateUserRequest> _createValidator;
    private readonly IValidator<UpdateUserRequest> _updateValidator;
    private readonly IValidator<PageRequest> _pageValidator;

    public UserService(
        IUserRepository userRepository,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator,
        IValidator<PageRequest> pageValidator)
    {
        _userRepository = userRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _pageValidator = pageValidator;
    }

    public async Task<UserResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByIdAsync(
            id,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{id}' was not found.");

        return user.ToResponse();
    }

    public async Task<PagedResult<UserResponse>> GetAllAsync(
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        await _pageValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var result = await _userRepository.GetUsersAsync(
            request,
            cancellationToken);

        return new PagedResult<UserResponse>
        {
            Items = result.Items
                .Select(x => x.ToResponse())
                .ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<UserResponse> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var existingUser = await _userRepository.GetUserByEmailAsync(
            request.Email,
            cancellationToken);

        if (existingUser is not null)
            throw new ConflictException(
                $"User with email '{request.Email}' already exists.");

        var user = request.ToEntity();

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        return user.ToResponse();
    }

    public async Task<UserResponse> UpdateAsync(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var user = await _userRepository.GetUserByIdAsync(
            id,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{id}' was not found.");

        var existingUser = await _userRepository.GetUserByEmailAsync(
            request.Email,
            cancellationToken);

        if (existingUser is not null && existingUser.Id != id)
            throw new ConflictException(
                $"User with email '{request.Email}' already exists.");

        request.UpdateEntity(user);

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        return user.ToResponse();
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByIdAsync(
            id,
            cancellationToken);

        if (user is null)
            throw new NotFoundException(
                $"User with id '{id}' was not found.");

        await _userRepository.DeleteAsync(
            id,
            cancellationToken);
    }
}