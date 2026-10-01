namespace TaskFlow.Application.DTO.Projects;

public class ProjectResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
}