using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoticeBoard.DTO;
using NoticeBoard.Models;
using NoticeBoard.Repositories;
using NoticeBoard.Services;

namespace NoticeBoard.Controllers;

[ApiController]
[Route("api/announcements")]
public class AnnouncementsController(IAnnouncementService service, IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Announcement>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] string? subCategory,
        [FromQuery] bool? status)
    {
        var items = await service.GetAllAsync(category, subCategory, status);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Announcement>> GetById(int id)
    {
        var item = await service.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAnnouncementDto dto)
    {
        var userId = await GetCurrentUserIdAsync();
        var created = await service.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAnnouncementDto dto)
    {
        var userId = await GetCurrentUserIdAsync();

        var existing = await service.GetByIdAsync(id);
        if (existing is null) return NotFound();
        if (existing.UserId != userId) return Forbid();

        try
        {
            await service.UpdateAsync(id, dto, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = await GetCurrentUserIdAsync();

        var existing = await service.GetByIdAsync(id);
        if (existing is null) return NotFound();
        if (existing.UserId != userId) return Forbid();

        try
        {
            await service.DeleteAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    private async Task<int> GetCurrentUserIdAsync()
    {
        var sub = User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("Missing 'sub' claim.");
        var email = User.FindFirstValue("email") ?? string.Empty;
        var name = User.FindFirstValue("name");

        var user = await userRepository.FindOrCreateAsync(sub, email, name);
        return user.Id;
    }
}
