namespace Ordering.Domain.ValueObjects;

public class Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string EmailAddress { get; }=default!;
    public string AddressLine { get;  } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string ZipoCode { get; } = default!;

    protected Address() { 

    }

    private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipoCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipoCode = zipoCode;
    }
    public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipoCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

        return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipoCode);
    }
}
