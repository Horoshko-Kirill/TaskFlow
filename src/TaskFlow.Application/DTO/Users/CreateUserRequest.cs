namespace TaskFlow.Application.DTO.Users;

public class CreateUserRequest
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
}