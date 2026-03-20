using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NoticeBoard.Models;

namespace NoticeBoard.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<User> FindOrCreateAsync(string googleSubject, string email, string? displayName)
    {
        using var db = new SqlConnection(_connectionString);

        return await db.QuerySingleAsync<User>(
            "dbo.sp_FindOrCreateUser",
            new { GoogleSubject = googleSubject, Email = email, DisplayName = displayName },
            commandType: CommandType.StoredProcedure);
    }
}
