using NoticeBoard.DTO;

namespace NoticeBoard.Repositories;

public interface ICategoryRepository
{
    Task<List<CategoryOptionDto>> GetCategoryOptionsAsync();
}

