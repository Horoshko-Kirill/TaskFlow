namespace TaskFlow.Application.DTO.Projects;

public class UpdateProjectRequest
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}