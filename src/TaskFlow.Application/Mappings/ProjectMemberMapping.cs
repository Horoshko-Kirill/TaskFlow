using TaskFlow.Application.DTO.ProjectMembers;
using TaskFlow.Domain.Models;

namespace TaskFlow.Application.Mappings;

public static class ProjectMemberMapping
{
    public static ProjectMember ToEntity(
        this AddProjectMemberRequest request,
        Guid projectId)
    {
        return new ProjectMember
        {
            ProjectId = projectId,
            UserId = request.UserId,
            JoinedAt = DateTime.UtcNow
        };
    }
    
    public static ProjectMemberResponse ToResponse(
        this ProjectMember member)
    {
        return new ProjectMemberResponse
        {
            ProjectId = member.ProjectId,
            UserId = member.UserId,
            JoinedAt = member.JoinedAt
        };
    }

    public static ProjectMemberListItemResponse ToListItemResponse(
        this ProjectMember member)
    {
        return new ProjectMemberListItemResponse
        {
            UserId = member.UserId,
            Name = member.User.Name,
            Email = member.User.Email,
            JoinedAt = member.JoinedAt
        };
    }
}