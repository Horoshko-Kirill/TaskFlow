namespace TaskFlow.Application.DTO.Users;

public class UpdateUserRequest
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
}