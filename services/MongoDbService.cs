using MongoDB.Driver;

public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(string connectionString, string databaseName)
    {
        try
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to initialize MongoDB service", e);
        }
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        try
        {
            return _database.GetCollection<T>(collectionName);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to get collection ", e);
        }
    }


}

