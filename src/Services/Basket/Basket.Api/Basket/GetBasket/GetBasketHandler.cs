namespace Basket.Api.Basket.GetBasket;

public record GetBasketQury(string userName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);
public class GetBasketQueryHandler : IQueryHandler<GetBasketQury, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQury request, CancellationToken cancellationToken)
    {
       return new GetBasketResult(new ShoppingCart("swn"));
    }
}
