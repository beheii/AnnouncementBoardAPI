using Microsoft.AspNetCore.Mvc;
using NoticeBoard.DTO;
using NoticeBoard.Repositories;

namespace NoticeBoard.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(ICategoryRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryOptionDto>>> GetAll()
    {
        var result = await repository.GetCategoryOptionsAsync();
        return Ok(result);
    }
}

