using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Projects;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var project = await _projectService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(project);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProjectResponse>>> GetAll(
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _projectService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<ActionResult<PagedResult<ProjectResponse>>> GetMy(
        [FromQuery] Guid userId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _projectService.GetByUserIdAsync(
            userId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(
        [FromQuery] Guid userId,
        CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectService.CreateAsync(
            userId,
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = project.Id },
            project);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> Update(
        Guid id,
        UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _projectService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}