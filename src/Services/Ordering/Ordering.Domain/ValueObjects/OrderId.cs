namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    public Guid Value { get; }
    public OrderId(Guid value) => Value = value;

    private static OrderId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("OrderId cannot be empty.");
        }
        return new OrderId(value);
    }
}
