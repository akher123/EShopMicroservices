namespace Ordering.Domain.ValueObjects;

public class Payment
{
    public string? CardName { get;  } = default!;
    public string? CardNumber { get; } = default!;
    public string Expiration { get; }=default!;

    public string CVV { get; } = default!;

    public string PaymentMethod { get; } = default!;

    protected Payment()
    {
        
    }

    public Payment(string? cardName, string? cardNumber, string expiration, string cVV, string paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cVV;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, string paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName, nameof(cardName));
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv, nameof(cvv));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
    }

}
