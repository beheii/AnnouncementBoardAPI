using NoticeBoard.DTO;

namespace NoticeBoard.Services;

public interface ICategoryService
{
    Task<List<CategoryOptionDto>> GetCategoryOptionsAsync();
}

