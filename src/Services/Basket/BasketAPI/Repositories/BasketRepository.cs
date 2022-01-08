using BasketAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BasketAPI.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache cache)
        {
            this._redisCache = cache;
        }
        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (basket != null)
            {
                return JsonConvert.DeserializeObject<ShoppingCart>(basket);
            }
            return null;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await this.GetBasket(basket.UserName);
        }
    }
}
