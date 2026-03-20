using NoticeBoard.DTO;

namespace NoticeBoard.Services;

public interface IAnnouncementService
{
    Task<IEnumerable<AnnouncementResponseDto>> GetAllAsync(string? category, string? subCategory, bool? status);
    Task<AnnouncementResponseDto?> GetByIdAsync(int id);
    Task<AnnouncementResponseDto> CreateAsync(CreateAnnouncementDto dto, int userId);
    Task UpdateAsync(int id, UpdateAnnouncementDto dto, int userId);
    Task DeleteAsync(int id, int userId);
}
