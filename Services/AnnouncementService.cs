using NoticeBoard.DTO;
using NoticeBoard.Models;
using NoticeBoard.Repositories;

namespace NoticeBoard.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _repository;

    public AnnouncementService(IAnnouncementRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Announcement>> GetAllAsync(string? category, string? subCategory, bool? status)
        => _repository.GetAllAsync(category, subCategory, status);

    public Task<Announcement?> GetByIdAsync(int id)
        => _repository.GetByIdAsync(id);

    public async Task<Announcement> CreateAsync(CreateAnnouncementDto dto, int userId)
    {
        var id = await _repository.CreateAsync(dto, userId);
        var created = await _repository.GetByIdAsync(id)
                      ?? throw new InvalidOperationException("Created announcement not found.");

        return created;
    }

    public async Task UpdateAsync(int id, UpdateAnnouncementDto dto, int userId)
    {
        var affected = await _repository.UpdateAsync(id, dto, userId);
        if (affected == 0)
        {
            throw new KeyNotFoundException($"Announcement {id} not found.");
        }
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var affected = await _repository.DeleteAsync(id, userId);
        if (affected == 0)
        {
            throw new KeyNotFoundException($"Announcement {id} not found.");
        }
    }
}

