using DataAccess.Enum;
using DiaryService.Models;

namespace DiaryService.DTO
{
    public class DiaryInsert
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Tag { get; set; } = new List<string>();
        public EmotionType Emotion { get; set; } = EmotionType.Normal;
        public List<ContentBlock> Content { get; set; } = new List<ContentBlock>();
    }
}
