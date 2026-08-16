namespace Catalog.Api.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Catagory, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x=>x.Id).NotEmpty().WithMessage("Product ID is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                            .Length(2,150).WithMessage("Name must be between 2 and 150 characters");
        RuleFor(x => x.Catagory).NotEmpty().WithMessage("Catagory is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("IamgeFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
public class UpdateProductCommandHandler(IDocumentSession session,ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
      //  logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", command);
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }
        product.Name = command.Name;
        product.Description = command.Description;
        product.Catagory = command.Catagory;
        product.Price = command.Price;
        product.ImageFile = command.ImageFile;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }
}
