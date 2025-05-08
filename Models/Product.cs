using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace e_commerce_cs.Models
{
  public class Product
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [JsonPropertyNameAttribute("image")]
    [BsonElement("image")]
    public string? Img { get; set; }

    [JsonPropertyNameAttribute("title")]
    [BsonElement("title")]
    public string Title { get; set; } = "";

    [JsonPropertyNameAttribute("qty")]
    [BsonElement("qty")]
    public int Qty { get; set; } = 0;

    [JsonPropertyNameAttribute("description")]
    [BsonElement("description")]
    public string Description { get; set; } = "";

    [JsonPropertyNameAttribute("rate")]
    [BsonElement("rate")]
    public int Rate { get; set; } = 0;

    [JsonPropertyNameAttribute("sales")]
    [BsonElement("sales")]
    public int Sales { get; set; } = 0;

    [JsonPropertyNameAttribute("tag")]
    [BsonElement("tag")]
    public int Tag { get; set; } = 0;

    [JsonPropertyNameAttribute("price")]
    [BsonElement("price")]
    public float Price { get; set; } = 0;

    [JsonPropertyNameAttribute("discount")]
    [BsonElement("discount")]
    public float Discount { get; set; } = 0;
  }
}
