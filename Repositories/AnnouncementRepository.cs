using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NoticeBoard.DTO;
using NoticeBoard.Models;

namespace NoticeBoard.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly string _connectionString;

    public AnnouncementRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<Announcement>> GetAllAsync(string? category, string? subCategory, bool? status)
    {
        using var db = CreateConnection();

        return await db.QueryAsync<Announcement>(
            "dbo.sp_GetAnnouncements",
            new { Category = category, SubCategory = subCategory, Status = status },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Announcement?> GetByIdAsync(int id)
    {
        using var db = CreateConnection();

        return await db.QuerySingleOrDefaultAsync<Announcement>(
            "dbo.sp_GetAnnouncementById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CreateAnnouncementDto dto)
    {
        using var db = CreateConnection();

        var newId = await db.ExecuteScalarAsync<int>(
            "dbo.sp_CreateAnnouncement",
            new
            {
                dto.Title,
                dto.Description,
                dto.Status,
                dto.Category,
                dto.SubCategory
            },
            commandType: CommandType.StoredProcedure);

        return newId;
    }

    public async Task<int> UpdateAsync(int id, UpdateAnnouncementDto dto)
    {
        using var db = CreateConnection();

        var affected = await db.ExecuteAsync(
            "dbo.sp_UpdateAnnouncement",
            new
            {
                Id = id,
                dto.Title,
                dto.Description,
                dto.Status,
                dto.Category,
                dto.SubCategory
            },
            commandType: CommandType.StoredProcedure);

        return affected;
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var db = CreateConnection();

        var affected = await db.ExecuteAsync(
            "dbo.sp_DeleteAnnouncement",
            new { Id = id },
            commandType: CommandType.StoredProcedure);

        return affected;
    }
}

