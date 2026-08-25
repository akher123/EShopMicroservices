namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtension
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider service)
    {
        var scope=service.CreateScope();
        var context= scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
          await SeedCustomerAsync(context);
          await SeedProdutAsync(context);
          await SeedOrderandItemsAsync(context);
    }

  

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if(!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
      
    }
    private static async Task SeedProdutAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }
    private static async Task SeedOrderandItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }

}
