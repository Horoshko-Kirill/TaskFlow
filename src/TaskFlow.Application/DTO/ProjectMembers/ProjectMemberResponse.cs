namespace TaskFlow.Application.DTO.ProjectMembers;

public class ProjectMemberResponse
{
    public Guid ProjectId { get; init; }
    public Guid UserId { get; init; }
    public DateTime JoinedAt { get; init; }
}