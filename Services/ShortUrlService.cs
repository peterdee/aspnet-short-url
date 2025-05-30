using Microsoft.Extensions.Options;
using MongoDB.Driver;
using aspnet_short_url.Models;

namespace aspnet_short_url.Services;

public class ShortUrlService
{
  private readonly IMongoCollection<ShortUrl> _shortUrlCollection;

  private readonly string ShorUrlCollectionName = "shortUrl";

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

    Console.WriteLine("create client");
    var mongoClient = new MongoClient(databaseConnectionString);
    var mongoDatabase = mongoClient.GetDatabase(databaseName);
    _shortUrlCollection = mongoDatabase.GetCollection<ShortUrl>(ShorUrlCollectionName);
  }

  public async Task<ShortUrl?> FindOneByShortIdAsync(string shortId) =>
      await _shortUrlCollection.Find(x => x.ShortId == shortId).FirstOrDefaultAsync();

  public async Task InsertOneAsync(ShortUrl value) =>
      await _shortUrlCollection.InsertOneAsync(value);

  public async Task UpdateOneByShortIdAsync(string shortId, ShortUrl updatedValue) =>
      await _shortUrlCollection.ReplaceOneAsync(x => x.ShortId == shortId, updatedValue);

  public async Task DeleteOneByShortIdAsync(string shortId) =>
      await _shortUrlCollection.DeleteOneAsync(x => x.ShortId == shortId);
}