using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extentions
    {
        public static async Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app)
        {
            using var scope=app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<DicountContext>();
            dbContext.Database.MigrateAsync();
            return app;
        }
    }
}
