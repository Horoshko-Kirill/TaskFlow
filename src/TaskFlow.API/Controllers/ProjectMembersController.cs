using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.ProjectMembers;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/members")]
public class ProjectMembersController : ControllerBase
{
    private readonly IProjectMemberService _projectMemberService;

    public ProjectMembersController(
        IProjectMemberService projectMemberService)
    {
        _projectMemberService = projectMemberService;
    }

    [HttpGet]
    public async Task<
        ActionResult<PagedResult<ProjectMemberListItemResponse>>> GetAll(
        Guid projectId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _projectMemberService.GetByProjectIdAsync(
            projectId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectMemberResponse>> Add(
        Guid projectId,
        AddProjectMemberRequest request,
        CancellationToken cancellationToken)
    {
        var member = await _projectMemberService.AddAsync(
            projectId,
            request,
            cancellationToken);

        return Ok(member);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Delete(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        await _projectMemberService.DeleteAsync(
            projectId,
            userId,
            cancellationToken);

        return NoContent();
    }
}