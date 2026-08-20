namespace Ordering.Domain.ValueObjects;

public record OderItemId
{
    public Guid Value { get; }

    public OderItemId(Guid value) => Value = value;

    public static OderItemId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("OderItemId cannot be empty.");
        }
        return new OderItemId(value);
    }
}
    