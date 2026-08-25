using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;



namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor:SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;
            foreach (var entity in context.ChangeTracker.Entries<IEntity>())
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedBy = "akher";
                    entity.Entity.CreatedAt = DateTime.UtcNow;
                }
                if(entity.State == EntityState.Added|| entity.State==EntityState.Modified|| entity.HasChanedOwnEitites())
                {
                    entity.Entity.LastModifiedBy = "akher";
                    entity.Entity.LastModified = DateTime.UtcNow;
                }
            }

        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


    }

    public static class Extensions
    {
        public static bool HasChanedOwnEitites(this EntityEntry entity) =>
            entity.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
