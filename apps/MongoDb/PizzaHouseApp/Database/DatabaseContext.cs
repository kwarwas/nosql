
using MongoDB.Bson;
using MongoDB.Driver;

namespace PizzaHouseApp.Database
{
    public class DatabaseContext
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public DatabaseContext(string database)
        {
            MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;

            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase(database);
        }

        public IMongoClient GetClient()
        {
            return _client;
        }

        public IMongoCollection<TModel> GetCollection<TModel>(string collectionName)
        {
            return _database.GetCollection<TModel>(string.IsNullOrWhiteSpace(collectionName)
                ? typeof(TModel).Name
                : collectionName);
        }
    }
}