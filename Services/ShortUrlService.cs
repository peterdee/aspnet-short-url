namespace aspnet_short_url.Services;

using MongoDB.Bson;
using MongoDB.Driver;
using aspnet_short_url.Constants;
using aspnet_short_url.Models;

public class ShortUrlService
{
  private readonly MongoClient _client;

  private readonly IMongoDatabase _db;

  private readonly IMongoCollection<ShortUrl> _collection;

  private readonly string shorUrlCollectionName = "shortUrl";

  public ShortUrlService()
  {
    var databaseConnectionString = Environment.GetEnvironmentVariable(EnvNames.DatabaseConnectionString);
    if (databaseConnectionString == "" || databaseConnectionString == null)
    {
      throw new Exception($"Missing required {EnvNames.DatabaseConnectionString} environment variable");
    }
    var databaseName = Environment.GetEnvironmentVariable(EnvNames.DatabaseName);
    if (databaseName == "" || databaseName == null)
    {
      throw new Exception($"Missing required {EnvNames.DatabaseName} environment variable");
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

  public async Task DeleteOneByShortIdAsync(string shortId) =>
    await _collection.DeleteOneAsync(item => item.ShortId == shortId);

  public async Task<ShortUrl?> FindOneByShortIdAsync(string shortId) =>
    await _collection.Find(item => item.ShortId == shortId).FirstOrDefaultAsync();

  public async Task<ShortUrl?> FindOneByShortIdAndUpdateAsync(
    string shortId,
    BsonDocument update
  )
  {
    var result = await _collection.FindOneAndUpdateAsync(item => item.ShortId == shortId, update);
    return result;
  }

  public async Task InsertOneAsync(ShortUrl value) =>
    await _collection.InsertOneAsync(value);

  public async Task UpdateOneByShortIdAsync(string shortId, ShortUrl updatedValue) =>
    await _collection.ReplaceOneAsync(item => item.ShortId == shortId, updatedValue);
}
