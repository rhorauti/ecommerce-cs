using e_commerce_cs.Middlewares;
using e_commerce_cs.Models;
using e_commerce_cs.MongoDB;

using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;

namespace e_commerce_cs.Repositories
{
  public class ProductRepository(MongoDbService mongoDbService) : ControllerBase
  {
    private readonly IMongoCollection<Product> _productsCollection = mongoDbService.GetCollection<Product>("products");
    public async Task<List<Product>> GetProductAsync()
    {
      try
      {
        return await _productsCollection.Find(_ => true).ToListAsync();
      }
      catch (Exception ex)
      {
        throw new HttpException(500, "Erro ao acessar os produtos: " + ex.Message);
      }
    }
  }
}
