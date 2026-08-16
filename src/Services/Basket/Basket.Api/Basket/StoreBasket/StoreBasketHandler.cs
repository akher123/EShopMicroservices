using Basket.Api.Data;

namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string userName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotEmpty().WithMessage("Cart Can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required!");
    }
}
public class StoreBasketCommandHandler(IBasketRepository repository): ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        var result= await repository.StoreBasketAsync(command.Cart, cancellationToken);

        return new StoreBasketResult(result.UserName);
    }
}
