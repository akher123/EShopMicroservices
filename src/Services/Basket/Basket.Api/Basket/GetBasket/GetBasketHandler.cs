using Basket.Api.Data;

namespace Basket.Api.Basket.GetBasket;

public record GetBasketQury(string userName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);
public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQury, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQury request, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetBasketAsync(request.userName, cancellationToken);
        if (shoppingCart is null)
        {
            throw new BasketNotFoundException(request.userName);
        }
        return new GetBasketResult(shoppingCart);
    }
}
