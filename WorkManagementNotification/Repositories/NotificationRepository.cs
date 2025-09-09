using MongoDB.Driver;
using WorkManagementNotification.Models;

namespace WorkManagementNotification.Repositories
{
    public class NotificationRepository
    {
        private readonly IMongoCollection<Notification> _collection;

        public NotificationRepository(IConfiguration configuration)
        {
            var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.Database);
            _collection = database.GetCollection<Notification>(settings.Collection);
        }

        public async Task<List<Notification>> GetByUserIdAsync(string userId) =>
            await _collection.Find(n => n.UserId == userId).ToListAsync();

        public async Task CreateAsync(Notification notification) =>
            await _collection.InsertOneAsync(notification);

        public async Task MarkAsReadAsync(string id)
        {
            var update = Builders<Notification>.Update.Set(n => n.IsRead, true);
            await _collection.UpdateOneAsync(n => n.Id == id, update);
        }
    }
}
