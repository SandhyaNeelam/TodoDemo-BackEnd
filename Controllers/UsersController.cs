using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEnd.Models;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Todo.Utilities;
using BackEnd.DTOs;

namespace BackEnd.Controllers;

[ApiController]
[Route("api/users")]

public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersRepository _users;
    private readonly IConfiguration _config;

    public UsersController(ILogger<UsersController> logger,
    IUsersRepository users, IConfiguration config)
    {
        _logger = logger;
        _users = users;
        _config = config;
    }


    [HttpPost("login")]
    public async Task<ActionResult<UsersLoginResDTO>> Login(
        [FromBody] UsersLoginDTO Data
    )
    {
        var existingUser = await _users.GetByName(Data.Name);

        if (existingUser is null)
            return NotFound();

        if (existingUser.Password != Data.Password)
            return BadRequest("Incorrect password");

        var token = Generate(existingUser);

        var res = new UsersLoginResDTO
        {
            Id = existingUser.Id,
            Name = existingUser.Name,
            Token = token
        };

        return Ok(res);
    }



    private string Generate(Users users)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            // new Claim(ClaimTypes.SerialNumber, users.Id.ToString()),
            new Claim(TodoConstants.Id, users.Id.ToString()),
            new Claim(TodoConstants.Name, users.Name),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


