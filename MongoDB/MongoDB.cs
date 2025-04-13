using MongoDB.Driver;

namespace e_commerce_cs.MongoDB
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"]
                ?? throw new ArgumentNullException(nameof(configuration), "String de conexão do MongoDB não encontrada.");
            var databaseName = configuration["MongoDB:DatabaseName"]
                ?? throw new ArgumentNullException(nameof(configuration), "Nome do banco de dados não encontrado.");
            var mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
