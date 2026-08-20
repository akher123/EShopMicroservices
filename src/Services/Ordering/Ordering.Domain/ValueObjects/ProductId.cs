namespace Ordering.Domain.ValueObjects;

public record ProductId
{
    public Guid Value { get; }
    public ProductId(Guid value) => Value = value;

    private static ProductId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("ProductId cannot be empty.");
        }
        return new ProductId(value);
    }
}
