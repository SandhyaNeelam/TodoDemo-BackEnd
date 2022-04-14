using BackEnd.Models;
using Dapper;
using Todo.Utilities;

namespace BackEnd.Repositories;

public interface ITodoRepository
{
    Task<TodoApp> Create(TodoApp Item);
    Task Update(TodoApp Item);
    Task Delete(int Id);
    Task<List<TodoApp>> GetAll();
    Task<TodoApp> GetById(int TodoId);
}

public class TodoRepository : BaseRepository, ITodoRepository
{
    public TodoRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<TodoApp> Create(TodoApp Item)
    {
        var query = $@"INSERT INTO {TableNames.todo_app} (title, user_id)
       VALUES(@Title, @UserId) RETURNING *";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<TodoApp>(query, Item);
    }

    public async Task Delete(int Id)
    {
        var query = $@"DELETE FROM {TableNames.todo_app} WHERE id = @Id";
        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { Id });
    }

    public async Task Update(TodoApp Item)
    {
        var query = $@"Update {TableNames.todo_app} SET title =@Title, is_complete= @IsComplete  WHERE id = @Id";
        using (var con = NewConnection)
            await con.ExecuteAsync(query, Item);
    }

    public async Task<List<TodoApp>> GetAll()
    {
        var query = $@"SELECT * FROM {TableNames.todo_app} ORDER BY created_at DESC";
        using (var con = NewConnection)
            return (await con.QueryAsync<TodoApp>(query)).AsList();
    }

    public async Task<TodoApp> GetById(int TodoId)
    {
        var query = $@"SELECT * FROM {TableNames.todo_app} WHERE id = @TodoId";  //id= @Id
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<TodoApp>(query, new { TodoId });  //new{Id = TodoId}
    }
}