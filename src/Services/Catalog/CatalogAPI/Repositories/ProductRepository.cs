using Catalog.API.Entities;
using CatalogAPI.Data;
using MongoDB.Driver;

namespace CatalogAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        ICatalogContext catalogCon;
        public ProductRepository(ICatalogContext cat)
        {
            this.catalogCon = cat;

        }

        public async Task CreateProduct(Product product)
        {
            await catalogCon.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(x => x.Id, id);
            DeleteResult ds = await catalogCon.Products.DeleteOneAsync(filterDefinition);
            return ds.IsAcknowledged && ds.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await catalogCon.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            return await catalogCon.Products.Find(x => x.Category == categoryName).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await catalogCon.Products.Find(x => x.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await catalogCon.Products.Find(x=>true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {

            var updateResult = await catalogCon
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
