using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DiaryService.Models
{
    public class UserDiary
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        [BsonElement("diary")]
        [BsonRepresentation(BsonType.String)]
        public List<Guid> DiaryIds { get; set; } = new List<Guid>();
    }
}
