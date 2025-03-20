using System.Text.Json.Serialization;

namespace DataAccess.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmotionType
    {
        SuperHappy, Happy, Normal, Sad, Angry, Shitty
    }
}
