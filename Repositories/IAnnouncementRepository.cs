using NoticeBoard.DTO;
using NoticeBoard.Models;

namespace NoticeBoard.Repositories;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> GetAllAsync(string? category, string? subCategory, bool? status);
    Task<Announcement?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateAnnouncementDto dto, int userId);
    Task<int> UpdateAsync(int id, UpdateAnnouncementDto dto, int userId);
    Task<int> DeleteAsync(int id, int userId);
}

