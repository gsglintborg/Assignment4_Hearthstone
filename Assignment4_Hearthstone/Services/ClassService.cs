using Assignment4_Hearthstone.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Assignment4_Hearthstone.Services
{
    public class ClassService
    {
        private readonly IMongoCollection<Class> _classCollection;

        public ClassService(IOptions<MongoDBSettings> MongoSettings)
        {
            MongoClient client = new MongoClient(MongoSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(MongoSettings.Value.DatabaseName);
            _classCollection = database.GetCollection<Class>(MongoSettings.Value.ClassCollectionName);
        }

        public async Task<List<Class>> GetAsync()
        {
            return await _classCollection.Find(x => true).ToListAsync();
        }

        // Seed the database with data from metadata.json
        // Code is inspired from Lesson 12 example code
        public void CreateClasses()
        {
            if (_classCollection.Find(x => true).Any())
                return;

            foreach (var path in new[] { "metadata.json" })
            {
                using var file = new StreamReader(path);
                var metadata = JsonSerializer.Deserialize<Metadata>(file.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });

                if (metadata == null || metadata.Classes == null)
                    return;

                _classCollection.InsertMany(metadata.Classes);
            }
        }
    }
}
