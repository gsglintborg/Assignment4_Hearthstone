namespace Assignment4_Hearthstone.Models
{
    public class MongoDBSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string CardCollectionName { get; set; }
        public string CardTypeCollectionName { get; set; }
        public string ClassCollectionName { get; set; }
        public string RarityCollectionName { get; set; }
        public string SetCollectionName { get; set; }
    }
}
