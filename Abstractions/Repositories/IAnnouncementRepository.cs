using NoticeBoard.DTO;

namespace NoticeBoard.Repositories;

public interface IAnnouncementRepository
{
    Task<IEnumerable<AnnouncementDataDto>> GetAllAsync(string? category, string? subCategory, bool? status);
    Task<AnnouncementDataDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateAnnouncementDto dto, int userId);
    Task<int> UpdateAsync(int id, UpdateAnnouncementDto dto, int userId);
    Task<int> DeleteAsync(int id, int userId);
}
