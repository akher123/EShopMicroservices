namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderResult(bool IsSuccess);
public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is require");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is require");
    }
}


