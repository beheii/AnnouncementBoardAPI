using NoticeBoard.DTO;
using NoticeBoard.Repositories;

namespace NoticeBoard.Services;

public class CategoryService(ICategoryRepository repository) : ICategoryService
{
    public Task<List<CategoryOptionDto>> GetCategoryOptionsAsync()
        => repository.GetCategoryOptionsAsync();
}

