namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    public Guid Value {  get;}
    public CustomerId(Guid value)=>Value = value;

    public static CustomerId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("CustomerId cannot be empty.");
        }
        return new CustomerId(value);
    }
}
