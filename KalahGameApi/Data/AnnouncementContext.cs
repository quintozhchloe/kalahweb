using KalahGameApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace KalahGameApi.Data
{
    public class AnnouncementContext
    {
        private readonly IMongoDatabase _database;

        public AnnouncementContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Announcement> Announcements => _database.GetCollection<Announcement>("Announcements");
    }
}
