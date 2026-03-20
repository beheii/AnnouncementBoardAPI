using System.Data;

namespace NoticeBoard.Repositories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
