using BackEnd.Settings;
using Npgsql;

namespace BackEnd.Repositories;

public class BaseRepository
{
    public readonly IConfiguration _configuration;
    public BaseRepository(IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        _configuration = configuration;
    }

    public NpgsqlConnection NewConnection => new NpgsqlConnection(_configuration
       .GetSection(nameof(PostgresSettings)).Get<PostgresSettings>().ConnectionString);
}