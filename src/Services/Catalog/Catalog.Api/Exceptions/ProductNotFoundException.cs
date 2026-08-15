namespace Catalog.Api.Exceptions;

public class ProductNotFoundException:Exception
{
    public ProductNotFoundException():base("Product not found!")
    {
        
    }
}
