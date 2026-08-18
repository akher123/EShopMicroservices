using Basket.Api.Data;
using Discount.Grpc;
using JasperFx.Events.Daemon;

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
public class StoreBasketCommandHandler(IBasketRepository repository,DiscountProtoService.DiscountProtoServiceClient  discountProto): ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {

        await DeductDiscount(command.Cart, cancellationToken);

        var result= await repository.StoreBasketAsync(command.Cart, cancellationToken);

        return new StoreBasketResult(result.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart,CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
            item.Price -= coupon.Amount;
        }
    }
}
