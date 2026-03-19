using NoticeBoard.DTO;
using NoticeBoard.Models;

namespace NoticeBoard.Services;

public interface IAnnouncementService
{
    Task<IEnumerable<Announcement>> GetAllAsync(string? category, string? subCategory, bool? status);
    Task<Announcement?> GetByIdAsync(int id);
    Task<Announcement> CreateAsync(CreateAnnouncementDto dto, int userId);
    Task UpdateAsync(int id, UpdateAnnouncementDto dto, int userId);
    Task DeleteAsync(int id, int userId);
}

