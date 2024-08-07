using KalahGameApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class Settings
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }
}

public class LeaderboardContext
{
    private readonly IMongoDatabase _database = null;

    public LeaderboardContext(IOptions<Settings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        if (client != null)
        {
            _database = client.GetDatabase(settings.Value.Database);
        }
    }

    public IMongoCollection<LeaderboardEntry> LeaderboardEntries
    {
        get
        {
            return _database.GetCollection<LeaderboardEntry>("LeaderboardEntries");
        }
    }

    public IMongoCollection<Announcement> Announcements => _database.GetCollection<Announcement>("Announcements");
}
