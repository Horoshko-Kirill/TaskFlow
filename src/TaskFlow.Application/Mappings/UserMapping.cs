using TaskFlow.Application.DTO.Users;
using TaskFlow.Domain.Models;

namespace TaskFlow.Application.Mappings;

public static class UserMapping
{
    public static User ToEntity(this CreateUserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void UpdateEntity(
        this UpdateUserRequest request,
        User user)
    {
        user.Name = request.Name;
        user.Email = request.Email;
    }

    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}