using BackEnd.Models;
using Dapper;
using Todo.Utilities;

namespace BackEnd.Repositories;

public interface IUsersRepository
{
    Task<Users> GetByName(string Name);
}

public class UsersRepository : BaseRepository, IUsersRepository
{
    public UsersRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Users> GetByName(string Name)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" WHERE name= @Name ";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { Name });

    }
}