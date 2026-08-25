namespace Ordering.Application.Orders.Queries.GetOrdersByNam;

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;
