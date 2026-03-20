using Microsoft.AspNetCore.Mvc;
using NoticeBoard.DTO;
using NoticeBoard.Models;
using NoticeBoard.Services;

namespace NoticeBoard.Controllers;

[ApiController]
[Route("api/announcements")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _service;
    private const int TestUserId = 1; // TEMP: use existing DB user for non-auth testing

    public AnnouncementsController(IAnnouncementService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Announcement>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] string? subCategory,
        [FromQuery] bool? status)
    {
        var items = await _service.GetAllAsync(category, subCategory, status);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Announcement>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAnnouncementDto dto)
    {
        var created = await _service.CreateAsync(dto, TestUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAnnouncementDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto, TestUserId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id, TestUserId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

