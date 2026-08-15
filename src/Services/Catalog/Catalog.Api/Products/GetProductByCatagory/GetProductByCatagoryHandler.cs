
namespace Catalog.Api.Products.GetProductByCatagory;

public record GetProductByCatagoryQuery(string Catagory) : IQuery<GetProductByCatagoryResult>;
public record GetProductByCatagoryResult(IEnumerable<Product> Products);
public class GetProductByCatagoryQueryHandler(IDocumentSession session, ILogger<GetProductByCatagoryQueryHandler> logger) : IQueryHandler<GetProductByCatagoryQuery, GetProductByCatagoryResult>
{
    public async Task<GetProductByCatagoryResult> Handle(GetProductByCatagoryQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
             .Where(x => x.Catagory.Contains(query.Catagory))
             .ToListAsync();

        return new GetProductByCatagoryResult(products);
    }
}
