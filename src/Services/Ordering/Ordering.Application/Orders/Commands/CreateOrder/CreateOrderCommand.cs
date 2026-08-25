namespace Ordering.Application.Orders.Commands.CreateOrder;
public record CreateOrderRequest(Guid Id);
public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderRequest>;

public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
    }
}

