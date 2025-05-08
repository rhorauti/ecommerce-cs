using System.Threading.Tasks;

using MongoDB.Driver;

namespace e_commerce_cs.MongoDB
{
  public class MongoDbService
  {
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration configuration)
    {
      string connectionString = configuration["MongoDB:ConnectionString"]
          ?? throw new ArgumentNullException(nameof(configuration), "String de conexão do MongoDB não encontrada.");
      string databaseName = configuration["MongoDB:DatabaseName"]
          ?? throw new ArgumentNullException(nameof(configuration), "Nome do banco de dados não encontrado.");
      MongoClient mongoClient = new(connectionString);
      _database = mongoClient.GetDatabase(databaseName);
      Console.WriteLine("database: " + _database);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
      return _database.GetCollection<T>(collectionName);
    }
  }
}
