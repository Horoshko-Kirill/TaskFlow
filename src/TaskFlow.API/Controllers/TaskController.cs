using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Tasks;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var task = await _taskService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(task);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TaskResponse>>> GetAll(
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _taskService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<ActionResult<PagedResult<TaskResponse>>> GetMy(
        [FromQuery] Guid userId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _taskService.GetByAssigneeIdAsync(
            userId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult<PagedResult<TaskResponse>>> GetByProject(
        Guid projectId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _taskService.GetByProjectIdAsync(
            projectId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("project/{projectId:guid}/assignee/{assigneeId:guid}")]
    public async Task<ActionResult<PagedResult<TaskResponse>>>
        GetByProjectAndAssignee(
            Guid projectId,
            Guid assigneeId,
            [FromQuery] PageRequest request,
            CancellationToken cancellationToken)
    {
        var result = await _taskService.GetByProjectAndAssigneeAsync(
            projectId,
            assigneeId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(
        CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var task = await _taskService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskResponse>> Update(
        Guid id,
        UpdateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var task = await _taskService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(task);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _taskService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}