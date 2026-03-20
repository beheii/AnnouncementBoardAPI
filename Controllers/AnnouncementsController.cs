using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoticeBoard.DTO;
using NoticeBoard.Repositories;

namespace NoticeBoard.Controllers;

[ApiController]
[Route("api/announcements")]
public class AnnouncementsController(IAnnouncementRepository repository, IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnnouncementResponseDto>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] string? subCategory,
        [FromQuery] bool? status)
    {
        var currentUserId = await TryGetCurrentUserIdAsync();
        var items = await repository.GetAllAsync(category, subCategory, status);
        return Ok(items.Select(x => ToResponseDto(x, currentUserId)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnnouncementResponseDto>> GetById(int id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        var currentUserId = await TryGetCurrentUserIdAsync();
        return Ok(ToResponseDto(item, currentUserId));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAnnouncementDto dto)
    {
        var userId = await GetCurrentUserIdAsync();
        var id = await repository.CreateAsync(dto, userId);
        var created = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Created announcement {id} not found.");

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToResponseDto(created, userId));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAnnouncementDto dto)
    {
        var userId = await GetCurrentUserIdAsync();
        var ownershipResult = await EnsureOwnerAsync(id, userId);
        if (ownershipResult is not null) return ownershipResult;

        var affected = await repository.UpdateAsync(id, dto, userId);
        if (affected == 0) return NotFound();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = await GetCurrentUserIdAsync();
        var ownershipResult = await EnsureOwnerAsync(id, userId);
        if (ownershipResult is not null) return ownershipResult;

        var affected = await repository.DeleteAsync(id, userId);
        if (affected == 0) return NotFound();
        return NoContent();
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

    private async Task<int?> TryGetCurrentUserIdAsync()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            return null;
        }

        try
        {
            return await GetCurrentUserIdAsync();
        }
        catch
        {
            return null;
        }
    }

    private async Task<ActionResult?> EnsureOwnerAsync(int announcementId, int userId)
    {
        var existing = await repository.GetByIdAsync(announcementId);
        if (existing is null) return NotFound();
        if (existing.UserId != userId) return Forbid();
        return null;
    }

    private static AnnouncementResponseDto ToResponseDto(AnnouncementDataDto item, int? currentUserId)
    {
        return new AnnouncementResponseDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            Status = item.Status,
            IsOwner = currentUserId.HasValue && item.UserId == currentUserId.Value,
            Category = item.Category,
            SubCategory = item.SubCategory
        };
    }
}
