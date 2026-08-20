namespace Ordering.Domain.Abstractions;

public interface IAggregate<T>: IAggregate,IEnity<T>
{

}

public interface IAggregate
{
    IReadOnlyList<IDomainEvent> DomainEvents {  get; }
    IDomainEvent[] ClearIDomainEvents();
}
