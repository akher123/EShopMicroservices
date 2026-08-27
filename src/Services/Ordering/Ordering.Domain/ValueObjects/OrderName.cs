namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int defaultLenght = 5;
    public string Value { get; }
    public OrderName(string value) => Value = value;

    public static OrderName Of(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(OrderName));
      //  ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, defaultLenght);
        return new OrderName(value);
    }

}
