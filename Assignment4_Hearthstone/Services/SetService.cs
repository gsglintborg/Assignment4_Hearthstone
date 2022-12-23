using Assignment4_Hearthstone.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Assignment4_Hearthstone.Services
{
    public class SetService
    {
        private readonly IMongoCollection<Set> _setCollection;

        public SetService(IOptions<MongoDBSettings> MongoSettings)
        {
            MongoClient client = new MongoClient(MongoSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(MongoSettings.Value.DatabaseName);
            _setCollection = database.GetCollection<Set>(MongoSettings.Value.SetCollectionName);
        }

        public async Task<List<Set>> GetAsync()
        {
            return await _setCollection.Find(x => true).ToListAsync();
        }

        // Seed the database with data from metadata.json
        // Code is inspired from Lesson 12 example code
        public void CreateSets()
        {
            foreach (var path in new[] { "metadata.json" })
            {
                using var file = new StreamReader(path);
                var metadata = JsonSerializer.Deserialize<Metadata>(file.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });

                if (metadata == null || metadata.Sets == null)
                    return;

                _setCollection.InsertMany(metadata.Sets);
            }
        }
    }
}
