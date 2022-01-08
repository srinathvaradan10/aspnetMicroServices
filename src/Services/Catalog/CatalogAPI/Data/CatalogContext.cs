using Catalog.API.Entities;
using MongoDB.Driver;

namespace CatalogAPI.Data
{
    public class CatalogContext : ICatalogContext
    {
        private IMongoClient _mongoClient;
        public CatalogContext(IConfiguration config)
        {
            var client = new MongoClient(connectionString: config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(config.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(config.GetValue<string>("DatabaseSettings:CollectionName"));

            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
