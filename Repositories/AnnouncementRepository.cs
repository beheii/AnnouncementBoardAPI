using System.Data;
using Dapper;
using NoticeBoard.DTO;

namespace NoticeBoard.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AnnouncementRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<AnnouncementResponseDto>> GetAllAsync(string? category, string? subCategory, bool? status)
    {
        using var db = _connectionFactory.CreateConnection();

        return await db.QueryAsync<AnnouncementResponseDto>(
            "dbo.sp_GetAnnouncements",
            new { Category = category, SubCategory = subCategory, Status = status },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<AnnouncementResponseDto?> GetByIdAsync(int id)
    {
        using var db = _connectionFactory.CreateConnection();

        return await db.QuerySingleOrDefaultAsync<AnnouncementResponseDto>(
            "dbo.sp_GetAnnouncementById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CreateAnnouncementDto dto, int userId)
    {
        using var db = _connectionFactory.CreateConnection();

        var newId = await db.ExecuteScalarAsync<int>(
            "dbo.sp_CreateAnnouncement",
            new
            {
                UserId = userId,
                dto.Title,
                dto.Description,
                dto.Status,
                dto.Category,
                dto.SubCategory
            },
            commandType: CommandType.StoredProcedure);

        return newId;
    }

    public async Task<int> UpdateAsync(int id, UpdateAnnouncementDto dto, int userId)
    {
        using var db = _connectionFactory.CreateConnection();

        var affected = await db.QuerySingleAsync<int>(
            "dbo.sp_UpdateAnnouncement",
            new
            {
                Id = id,
                UserId = userId,
                dto.Title,
                dto.Description,
                dto.Status,
                dto.Category,
                dto.SubCategory
            },
            commandType: CommandType.StoredProcedure);

        return affected;
    }

    public async Task<int> DeleteAsync(int id, int userId)
    {
        using var db = _connectionFactory.CreateConnection();

        var affected = await db.QuerySingleAsync<int>(
            "dbo.sp_DeleteAnnouncement",
            new { Id = id, UserId = userId },
            commandType: CommandType.StoredProcedure);

        return affected;
    }
}

