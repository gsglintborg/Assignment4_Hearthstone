using Assignment4_Hearthstone.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Assignment4_Hearthstone.Services
{
    public class RarityService
    {
        private readonly IMongoCollection<Rarity> _rarityCollection;

        public RarityService(IOptions<MongoDBSettings> MongoSettings)
        {
            MongoClient client = new MongoClient(MongoSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(MongoSettings.Value.DatabaseName);
            _rarityCollection = database.GetCollection<Rarity>(MongoSettings.Value.RarityCollectionName);
        }

        public async Task<List<Rarity>> GetAsync()
        {
            return await _rarityCollection.Find(x => true).ToListAsync();
        }

        // Seed the database with data from metadata.json
        // Code is inspired from Lesson 12 example code
        public void CreateRarities()
        {
            foreach (var path in new[] { "metadata.json" })
            {
                using var file = new StreamReader(path);
                var metadata = JsonSerializer.Deserialize<Metadata>(file.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });

                if (metadata == null || metadata.Rarities == null)
                    return;

                _rarityCollection.InsertMany(metadata.Rarities);
            }
        }
    }
}
