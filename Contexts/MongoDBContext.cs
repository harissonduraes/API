using MongoDB.Driver;

namespace API.Contexts
{
    public class MongoDBContext
    {
        public MongoDBContext()
        {
            var connection = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connection);

            _database = client.GetDatabase("API");
        }

        private IMongoDatabase _database;

        public IMongoCollection<TModel> GetCollection<TModel>(string name)
        {
            return _database.GetCollection<TModel>(name);
        }
    }
}
