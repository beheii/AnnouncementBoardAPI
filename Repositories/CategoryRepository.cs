using Dapper;
using System.Data;
using NoticeBoard.DTO;
using NoticeBoard.Infrastructure.Data;

namespace NoticeBoard.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CategoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<CategoryOptionDto>> GetCategoryOptionsAsync()
    {
        using var db = _connectionFactory.CreateConnection();
        var rows = await db.QueryAsync<(string CategoryName, string? SubCategoryName)>(
            "dbo.sp_GetCategoryOptions",
            commandType: CommandType.StoredProcedure);

        return rows
            .GroupBy(r => r.CategoryName)
            .Select(g => new CategoryOptionDto
            {
                Name = g.Key,
                SubCategories = g.Where(x => !string.IsNullOrWhiteSpace(x.SubCategoryName))
                                 .Select(x => x.SubCategoryName!)
                                 .Distinct()
                                 .ToList()
            })
            .ToList();
    }
}

