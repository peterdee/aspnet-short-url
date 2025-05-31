using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace aspnet_short_url.Models;

public class ShortUrlPostDto
{
  [JsonPropertyName("url")]
  public string Url { get; set; } = null!;
}

public class ShortUrl
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string? Id { get; set; }

  [BsonElement("OriginalUrl")]
  [JsonPropertyName("originalUrl")]
  public string OriginalUrl { get; set; } = null!;

  [BsonElement("RedirectCount")]
  [JsonPropertyName("redirectCount")]
  public uint RedirectCount { get; set; } = 0;

  [BsonElement("ShortId")]
  [JsonPropertyName("shortId")]
  public string ShortId { get; set; } = null!;
}
