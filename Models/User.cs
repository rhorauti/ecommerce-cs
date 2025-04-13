using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace e_commerce_cs.Models
{
  public class User
  {
    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)] 
    public string? _id { get; set; } = ObjectId.GenerateNewId().ToString();
    [BsonElement("name")]
    public required string Name { get; set; }
    [BsonElement("email")]
    public required string Email { get; set; }
    [JsonIgnore]
    [BsonElement("password")]
    public string Password { get; set; } = "";
    [BsonElement("avatar")]
    public string? Avatar { get; set; } = null;
    [JsonIgnore]
    [BsonElement("emailConfirmed")]
    public bool EmailConfirmed { get; set; } = false;
  }
}