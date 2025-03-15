using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiaryService.Models
{
    public class ContentBlock
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class Diary
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("name")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("tag")]
        public List<string> Tag { get; set; } = new List<string>();

        [BsonElement("emotion")]
        public string Emotion { get; set; } = string.Empty;

        [BsonElement("content")]
        public List<ContentBlock> Content { get; set; } = new List<ContentBlock>();
        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Contruction
        public Diary() { }

        public Diary(string title, List<string> tag, string emotion, List<ContentBlock> content)
        {
            Id = Guid.NewGuid();
            Title = title;
            Tag = tag;
            Emotion = emotion;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
