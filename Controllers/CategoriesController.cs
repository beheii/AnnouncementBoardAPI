using Microsoft.AspNetCore.Mvc;
using NoticeBoard.DTO;
using NoticeBoard.Services;

namespace NoticeBoard.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(ICategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryOptionDto>>> GetAll()
    {
        var result = await service.GetCategoryOptionsAsync();
        return Ok(result);
    }
}

