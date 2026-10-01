using TaskFlow.Application.DTO.Projects;
using TaskFlow.Domain.Models;

namespace TaskFlow.Application.Mappings;

public static class ProjectMapping
{
    public static Project ToEntity(this CreateProjectRequest request)
    {
        return new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public static void UpdateEntity(
        this UpdateProjectRequest request,
        Project project)
    {
        project.Name = request.Name;
        project.Description = request.Description;
    }

    public static ProjectResponse ToResponse(this Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
    }
}