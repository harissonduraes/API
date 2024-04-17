using MongoDB.Driver;

namespace API.Contexts
{
    public class MongoDBContext
    {
        public MongoDBContext()
        {
            var connection = "mongodb://localhost:27017";
            MongoClient client = new (connection);

            _database = client.GetDatabase("API");
        }

        public IMongoCollection<TModel> GetCollection<TModel>(string name)
        {
            return _database.GetCollection<TModel>(name);
        }

        private IMongoDatabase _database;
    }
}
