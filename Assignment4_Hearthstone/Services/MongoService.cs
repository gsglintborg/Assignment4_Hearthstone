using Assignment4_Hearthstone.Models;
using MongoDB.Driver;
using System.Text.Json;

namespace Assignment4_Hearthstone.Services
{
    public class MongoService
    {
        private readonly MongoClient _client;
        public MongoService()
        {
            _client = new MongoClient("mongodb://root:example@mongo:27017");
            var db = _client.GetDatabase("HearthstoneDB");
            if (_client.GetDatabase("HearthstoneDB").ListCollections().ToList().Count == 0)
            {
                var collection = db.GetCollection<Card>("Cards");
                foreach (var path in new[] { "cards.json", "metadata.json" })
                {
                    using var file = new StreamReader(path);
                    var cards = JsonSerializer.Deserialize<List<Card>>(file.ReadToEnd(), new JsonSerializerOptions
                        { PropertyNameCaseInsensitive = true });
                    collection.InsertMany(cards);
                }
            }
        }
        public MongoClient Client
        {
            get { return _client; }
        }
    }
}
