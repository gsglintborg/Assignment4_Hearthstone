using Assignment4_Hearthstone.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Assignment4_Hearthstone.Services
{
    public class CardTypeService
    {
        private readonly IMongoCollection<CardType> _typeCollection;

        public CardTypeService(IOptions<MongoDBSettings> MongoSettings)
        {
            MongoClient client = new MongoClient(MongoSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(MongoSettings.Value.DatabaseName);
            _typeCollection = database.GetCollection<CardType>(MongoSettings.Value.CardTypeCollectionName);
        }

        public async Task<List<CardType>> GetAsync()
        {
            return await _typeCollection.Find(x => true).ToListAsync();
        }

        // Seed the database with data from metadata.json
        // Code is inspired from Lesson 12 example code
        public void CreateCardTypes()
        {
            if (_typeCollection.Find(x => true).Any())
                return;

            foreach (var path in new[] { "metadata.json" })
            {
                using var file = new StreamReader(path);
                var metadata = JsonSerializer.Deserialize<Metadata>(file.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });

                if (metadata == null || metadata.Types == null)
                    return;
                
                _typeCollection.InsertMany(metadata.Types);
            }
        }
    }
}
