namespace TaskFlow.Application.DTO.ProjectMembers;

public class ProjectMemberListItemResponse
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public DateTime JoinedAt { get; init; }
}