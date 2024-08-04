using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KalahGameApi.Models
{
    public class LeaderboardEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("playerName")]
        public string PlayerName { get; set; }

        [BsonElement("score")]
        public int Score { get; set; }
    }
}
