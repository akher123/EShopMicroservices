   namespace Ordering.Infrastructure.Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
            customerId => customerId.Value,
            dbId => CustomerId.Of(dbId)
            );
        builder.Property(c=>c.Name).HasMaxLength(100).IsRequired();
        builder.Property(x=>x.Email).HasMaxLength(255);
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
