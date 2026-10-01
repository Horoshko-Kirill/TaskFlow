using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTO.Users;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Models.Pagination;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UserResponse>>> GetAll(
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetAllAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            user);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Update(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}