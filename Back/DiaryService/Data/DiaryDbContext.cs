using MongoDB.Driver;
using Microsoft.Extensions.Options;
using DiaryService.Models;
using DiaryService.DTO;

namespace DiaryService.Data
{
    public class DiaryDbContext
    {
        private readonly IMongoCollection<Diary> _diaryCollection;

        public DiaryDbContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _diaryCollection = database.GetCollection<Diary>("diary");
        }

        public async Task<List<Diary>> GetAllPostsAsync()
        {
            return await _diaryCollection.Find(_ => true).ToListAsync();
        }
        public async Task<List<Diary>> GetAllByListAsync(List<Guid> listId)
        {
            var filter = Builders<Diary>.Filter.In(d => d.Id, listId);
            return await _diaryCollection.Find(filter).ToListAsync();
        }

        public async Task<Diary?> GetPostByIdAsync(Guid id)
        {
            return await _diaryCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(DiaryInsert diary)
        {
            Diary obj = new Diary(diary.Title, diary.Tag, diary.Emotion, diary.Content);
            await _diaryCollection.InsertOneAsync(obj);
        }
    }
}
