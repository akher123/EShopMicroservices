using Catalog.Api.Products.GetProducts;

namespace Catalog.Api.Products.GetProductByCatagory;

public record GetProductByCatagoryResponse(IEnumerable<Product> Products);

public class GetProductByCatagoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("products/catagory/{catagory}", async (string catagory, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCatagoryQuery(catagory));
            var response = result.Adapt<GetProductByCatagoryResponse>();
            return Results.Ok(response);

        })
         .WithName("GetProductByCatagory")
         .Produces<GetProductByCatagoryResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Get Product By Catagory")
         .WithDescription("Get Product By Catagory");
    }
}
