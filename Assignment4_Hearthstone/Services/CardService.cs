using MongoDB.Driver;
using Assignment4_Hearthstone.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace Assignment4_Hearthstone.Services
{
    public class CardService
    {
        private readonly IMongoCollection<Card> _cardCollection;
        private readonly IMongoCollection<CardType> _cardTypeCollection;
        private readonly IMongoCollection<Class> _classCollection;
        private readonly IMongoCollection<Rarity> _rarityCollection;
        private readonly IMongoCollection<Set> _setCollection;

        public CardService(IOptions<MongoDBSettings> MongoSettings)
        {
            MongoClient client = new MongoClient(MongoSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(MongoSettings.Value.DatabaseName);
            _cardCollection = database.GetCollection<Card>(MongoSettings.Value.CardCollectionName);
            _cardTypeCollection = database.GetCollection<CardType>(MongoSettings.Value.CardTypeCollectionName);
            _classCollection = database.GetCollection<Class>(MongoSettings.Value.ClassCollectionName);
            _rarityCollection = database.GetCollection<Rarity>(MongoSettings.Value.RarityCollectionName);
            _setCollection = database.GetCollection<Set>(MongoSettings.Value.SetCollectionName);
        }

        // Seed the database with data from cards.json
        // Code is inspired from Lesson 12 example code
        public void CreateCards()
        {
            if (_cardCollection.Find(x => true).Any())
                return;
            
            foreach (var path in new[] { "cards.json" })
            {
                using var file = new StreamReader(path);
                var cards = JsonSerializer.Deserialize<List<Card>>(file.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true });

                _cardCollection.InsertMany(cards);
            }
        }

        // GET endpoint with different query parameters
        public async Task<List<CardMappedToMetadataDTO>> GetCardsByQueryAsync(QueryParameters param)
        {
            var result = new List<Card>();
            var filter = Builders<Card>.Filter.Empty;

            // Parameter for filtering cards by Set
            if (param.SetId != null)
            { filter &= Builders<Card>.Filter.Eq(x => x.SetId, param.SetId); }

            // Parameter for filtering cards by Artist
            if (param.Artist != null)
            { filter &= Builders<Card>.Filter.Eq(x => x.Artist, param.Artist); }

            // Parameter for filtering cards by Class
            if (param.ClassId != null)
            { filter &= Builders<Card>.Filter.Eq(x => x.ClassId, param.ClassId); }

            // Parameter for filtering cards by Rarity
            if (param.RarityId != null)
            { filter &= Builders<Card>.Filter.Eq(x => x.RarityId, param.RarityId); }

            // Parameter for pagination, each page must have at most 100 entries
            if (param.Page != null)
            { result = await _cardCollection.Find(filter).Skip(param.Page > 0 ? ((param.Page - 1) * 100) : 0).Limit(100).ToListAsync(); }
            else
            { result = await _cardCollection.Find(filter).ToListAsync(); }

            return MapCardsToMetadata(result);
        }

        // Maps the cards to the metadata by Id, so that the string value is returned instead of the Id
        public List<CardMappedToMetadataDTO> MapCardsToMetadata(List<Card> cards)
        {
            var dto = new List<CardMappedToMetadataDTO>();
            
            foreach (var card in cards)
            {
                dto.Add(new CardMappedToMetadataDTO
                {
                    Id = card.Id,
                    Name = card.Name,
                    Class = (from c in _classCollection.AsQueryable()
                             where c.Id == card.ClassId
                             select c.Name).FirstOrDefault(),
                    Type = (from t in _cardTypeCollection.AsQueryable()
                            where t.Id == card.TypeId
                            select t.Name).FirstOrDefault(),
                    Set = (from s in _setCollection.AsQueryable()
                           where s.Id == card.SetId
                           select s.Name).FirstOrDefault(),
                    SpellSchoolId = card.SpellSchoolId,
                    Rarity = (from r in _rarityCollection.AsQueryable()
                              where r.Id == card.RarityId
                              select r.Name).FirstOrDefault(),
                    Health = card.Health,
                    Attack = card.Attack,
                    ManaCost = card.ManaCost,
                    Artist = card.Artist,
                    Text = card.Text,
                    FlavorText = card.FlavorText
                });
            }
            return dto;
        }
    }
}
