namespace TaskFlow.Application.DTO.Projects;

public class CreateProjectRequest
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}