using e_commerce_cs.DTOs;
using e_commerce_cs.Models;
using e_commerce_cs.MongoDB;

using MongoDB.Bson;
using MongoDB.Driver;

namespace e_commerce_cs.Repositories
{
  public class AuthRepository(MongoDbService mongoDbService)
  {
    private readonly IMongoCollection<User> _userCollection = mongoDbService.GetCollection<User>("users");

    public FilterDefinition<User> GetUserFilter(string email)
    {
      return Builders<User>.Filter.Eq(u => u.Email, email);
    }

    public async Task<User> GetUser(string email)
    {
      FilterDefinition<User> filter = GetUserFilter(email);
      return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User> SaveUser(User user)
    {
      FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
      var options = new ReplaceOptions { IsUpsert = true };
      ReplaceOneResult result = await _userCollection.ReplaceOneAsync(filter, user, options);
      if (result.MatchedCount > 0)
      {
        return user;
      }
      return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User> UpdateEmailConfirmedAsync(User user)
    {
      FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u._id, user._id);
      UpdateDefinition<User> update = Builders<User>.Update.Set(u => u.EmailConfirmed, true);

      await _userCollection.UpdateOneAsync(filter, update);
      return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<User> UpdatePasswordAsync(User user, string password)
    {
      FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u._id, user._id);
      UpdateDefinition<User> update = Builders<User>.Update.Set(u => u.Password, password);

      await _userCollection.UpdateOneAsync(filter, update);
      return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }
  }
}
