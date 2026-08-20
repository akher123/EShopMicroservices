using MediatR;

namespace Ordering.Domain.Abstractions
{
    public interface IDomainEvent:INotification 
    {
        Guid EventId=>Guid.NewGuid();
        public DateTime CreatedOn => DateTime.Now;

        public string EventType => GetType().AssemblyQualifiedName!;

    }
}
