using BuildingBlocks.Exceptions;

namespace Catalog.Api.Exceptions;

public class ProductNotFoundException:NotFoundException
{
    public ProductNotFoundException(Guid Id):base("Product",Id)
    {
        
    }
}
