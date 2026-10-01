namespace TaskFlow.Application.DTO.Users;

public class UserResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}