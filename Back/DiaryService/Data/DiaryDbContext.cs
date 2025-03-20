using MongoDB.Driver;
using DiaryService.Models;
using DiaryService.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;

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

        public async Task RemoveAsync(Guid id)
        {
            var filter = Builders<Diary>.Filter.Eq(d => d.Id, id);
            var result = await _diaryCollection.DeleteOneAsync(filter);
            if (result.DeletedCount == 0)
            {
                throw new Exception("Cant find the record");
            }
        }

        public async Task<int> RemoveAllByList(List<Guid> listId)
        {
            var filter = Builders<Diary>.Filter.In(d => d.Id, listId);
            var result = await _diaryCollection.DeleteManyAsync(filter);
            return (int)result.DeletedCount;
        }

        public async Task EditAsync(Guid id, DiaryInsert diary)
        {
            var filter = Builders<Diary>.Filter.Eq(d => d.Id, id);
            var update = Builders<Diary>.Update
                .Set(d => d.Title, diary.Title)
                .Set(d => d.Tag, diary.Tag)
                .Set(d => d.Emotion, diary.Emotion)
                .Set(d => d.Content, diary.Content)
                .Set(d => d.UpdatedAt, DateTime.UtcNow);

            await _diaryCollection.UpdateOneAsync(filter, update);
        }
    }
}
