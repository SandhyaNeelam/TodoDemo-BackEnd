using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd.DTOs;

public record TodoDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("is_complete")]
    public bool IsComplete { get; set; }


}

public record TodoCreateDTO
{
    [JsonPropertyName("title")]
    [Required]
    [MinLength(4)]
    [MaxLength(255)]
    public string Title { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }


}

public record TodoUpdateDTO
{
    [JsonPropertyName("title")]
    [MinLength(4)]
    [MaxLength(255)]
    public string Title { get; set; } = null;

    [JsonPropertyName("is-Complete")]
    public bool? IsComplete { get; set; } = null;
}