using BuildingBlocks.Exceptions;

namespace Basket.Api.Exceptions
{
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string message) : base("basket",message)
        {
        }
    }
}
