using NoticeBoard.DTO;

namespace NoticeBoard.Repositories;

public interface IAnnouncementRepository
{
    Task<IEnumerable<AnnouncementResponseDto>> GetAllAsync(string? category, string? subCategory, bool? status);
    Task<AnnouncementResponseDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateAnnouncementDto dto, int userId);
    Task<int> UpdateAsync(int id, UpdateAnnouncementDto dto, int userId);
    Task<int> DeleteAsync(int id, int userId);
}

