namespace Ordering.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderResult(bool IsSuccess);
public record DeleteOrderCommand(Guid orderId) : ICommand<DeleteOrderResult>;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.orderId).NotEmpty().WithMessage("OrderId is require");
    }
}