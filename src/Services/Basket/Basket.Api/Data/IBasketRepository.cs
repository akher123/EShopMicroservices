namespace Basket.Api.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken);
    Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken);
    Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken);
}
