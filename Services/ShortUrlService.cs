using MongoDB.Driver;
using aspnet_short_url.Models;

namespace aspnet_short_url.Services;

public class ShortUrlService
{
  private readonly MongoClient _client;

  private readonly IMongoDatabase _db;

  private readonly IMongoCollection<ShortUrl> _collection;

  private readonly string shorUrlCollectionName = "shortUrl";

  public ShortUrlService()
  {
    // get database connection string & database name
    var databaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
    if (databaseConnectionString == "" || databaseConnectionString == null)
    {
      throw new Exception("Missing required DATABASE_CONNECTION_STRING environment variable");
    }
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
    if (databaseName == "" || databaseName == null)
    {
      throw new Exception("Missing required DATABASE_NAME environment variable");
    }

    _client = new MongoClient(databaseConnectionString);
    _db = _client.GetDatabase(databaseName);
    _collection = _db.GetCollection<ShortUrl>(shorUrlCollectionName);

    new Action(async () => await PingDatabase())();
  }

  private async Task PingDatabase()
  {
    try
    {
      await _db.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping: 1}");
      Console.WriteLine("Connected to the database");
    }
    catch
    {
      throw new Exception("Could not ping the database");
    }
  }

  public async Task<ShortUrl?> FindOneByShortIdAsync(string shortId) =>
      await _collection.Find(item => item.ShortId == shortId).FirstOrDefaultAsync();

  public async Task InsertOneAsync(ShortUrl value) =>
      await _collection.InsertOneAsync(value);

  public async Task UpdateOneByShortIdAsync(string shortId, ShortUrl updatedValue) =>
      await _collection.ReplaceOneAsync(item => item.ShortId == shortId, updatedValue);

  public async Task DeleteOneByShortIdAsync(string shortId) =>
      await _collection.DeleteOneAsync(item => item.ShortId == shortId);
}