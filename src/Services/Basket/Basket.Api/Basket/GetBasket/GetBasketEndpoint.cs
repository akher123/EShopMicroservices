namespace Basket.Api.Basket.GetBasket;

public record GetBasketResponse(ShoppingCart Cart);
public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var query=new GetBasketQury(userName);
            var result=await sender.Send(query);
            var response= result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
         .WithName("GetBasketByUserName")
         .Produces<GetBasketResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Get Basket by username")
         .WithDescription("Get Basket by username");
    }
}
