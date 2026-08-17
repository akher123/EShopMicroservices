using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data;

public class CachedBasketRepository
    (IBasketRepository repository, IDistributedCache cache)
    : IBasketRepository
{
   
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var cachedBasket=await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
           return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;  
        }
        var basket=await repository.GetBasketAsync(userName,cancellationToken);
        await cache.SetStringAsync(userName,JsonSerializer.Serialize(basket));

       return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken)
    {
        await cache.SetStringAsync(basket.UserName,JsonSerializer.Serialize(basket),cancellationToken);

        await repository.StoreBasketAsync(basket, cancellationToken);

        return basket;

    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken)
    {
        await repository.DeleteBasketAsync(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }

}
