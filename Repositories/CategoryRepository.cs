using Dapper;
using Microsoft.Data.SqlClient;
using NoticeBoard.DTO;

namespace NoticeBoard.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly string _connectionString;

    public CategoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<List<CategoryOptionDto>> GetCategoryOptionsAsync()
    {
        const string sql = """
            SELECT c.Name AS CategoryName, sc.Name AS SubCategoryName
            FROM dbo.Categories c
            LEFT JOIN dbo.SubCategories sc ON sc.CategoryId = c.Id
            ORDER BY c.Name, sc.Name;
            """;

        using var db = CreateConnection();
        var rows = await db.QueryAsync<(string CategoryName, string? SubCategoryName)>(sql);

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

