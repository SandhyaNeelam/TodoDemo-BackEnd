using System.Security.Claims;
using BackEnd.DTOs;
using BackEnd.Models;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Utilities;

namespace BackEnd.Controllers;

[ApiController]
[Route("api/todo_app")]
[Authorize]

public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoRepository _todo;

    public TodoController(ILogger<TodoController> logger,
    ITodoRepository todo)
    {
        _logger = logger;
        _todo = todo;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TodoConstants.Id).First().Value);

    }

    [HttpPost]
    public async Task<ActionResult<TodoApp>> CreateTodo([FromBody] TodoCreateDTO Data)
    {
        // var claims = HttpContext.User.Claims;
        // var User_Id = Convert.ToInt32(claims.Where(x => x.Type == TodoConstants.Id).First().Value);  o(r)
        var User_Id = GetUserIdFromClaims(User.Claims);

        var toCreateItem = new TodoApp
        {
            Title = Data.Title.Trim(),
            // UserId = Data.UserId,
            UserId = User_Id,
        };
        var createdItem = await _todo.Create(toCreateItem);
        return StatusCode(201, createdItem);
    }


    [HttpPut("{todo_id}")]
    public async Task<ActionResult> UpdateTodo([FromRoute] int todo_id,
        [FromBody] TodoUpdateDTO Data)
    {
        var User_Id = GetUserIdFromClaims(User.Claims);

        var existingItem = await _todo.GetById(todo_id);
        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != User_Id)
            return StatusCode(403, "You cannot update others Todo");

        var toUpdateItem = existingItem with
        {
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),
            IsComplete = !Data.IsComplete.HasValue ? existingItem.IsComplete : Data.IsComplete.Value,
        };
        await _todo.Update(toUpdateItem);
        return NoContent();
    }


    [HttpDelete("{todo_id}")]
    public async Task<ActionResult> DeleteTodo(int todo_id)
    {
        var User_Id = GetUserIdFromClaims(User.Claims);


        var existingItem = await _todo.GetById(todo_id);
        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != User_Id)
            return StatusCode(403, "You cannot delete others Todo");

        await _todo.Delete(todo_id);
        return NoContent();
    }

    [HttpGet()]
    public async Task<ActionResult<List<TodoApp>>> GetAllTodos()
    {
        var allTodo = await _todo.GetAll();
        return Ok(allTodo);
    }


}