using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BackEnd.Models;

namespace BackEnd.DTOs;

public record UsersLoginDTO
{
    [JsonPropertyName("name")]
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; }

    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }
}
public record UsersLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }


}
