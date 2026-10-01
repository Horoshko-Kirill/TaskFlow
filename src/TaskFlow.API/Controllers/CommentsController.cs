using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Comments;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("tasks/{taskId:guid}/comments")]
    public async Task<ActionResult<PagedResult<CommentResponse>>> GetByTask(
        Guid taskId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _commentService.GetByTaskIdAsync(
            taskId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("comments")]
    public async Task<ActionResult<CommentResponse>> Create(
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await _commentService.CreateAsync(
            request,
            cancellationToken);

        return Ok(comment);
    }

    [HttpPut("comments/{id:guid}")]
    public async Task<ActionResult<CommentResponse>> Update(
        Guid id,
        UpdateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await _commentService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(comment);
    }

    [HttpDelete("comments/{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _commentService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}