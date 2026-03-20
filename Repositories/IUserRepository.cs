using NoticeBoard.Models;

namespace NoticeBoard.Repositories;

public interface IUserRepository
{
    Task<User> FindOrCreateAsync(string googleSubject, string email, string? displayName);
}
