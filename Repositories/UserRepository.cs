using System.Data;
using Dapper;
using NoticeBoard.Infrastructure.Data;
using NoticeBoard.Models;

namespace NoticeBoard.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User> FindOrCreateAsync(string googleSubject, string email, string? displayName)
    {
        using var db = _connectionFactory.CreateConnection();

        return await db.QuerySingleAsync<User>(
            "dbo.sp_FindOrCreateUser",
            new { GoogleSubject = googleSubject, Email = email, DisplayName = displayName },
            commandType: CommandType.StoredProcedure);
    }
}
