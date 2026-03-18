using NoticeBoard.DTO;
using NoticeBoard.Models;

namespace NoticeBoard.Repositories;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> GetAllAsync(string? category, string? subCategory, bool? status);
    Task<Announcement?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateAnnouncementDto dto);
    Task<int> UpdateAsync(int id, UpdateAnnouncementDto dto);
    Task<int> DeleteAsync(int id);
}

