using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasConversion(
            orderId => orderId.Value,
            dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
			.IsRequired();

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(
            o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName)
                    //.HasColumnName(nameof(Order.ShippingAddress.FirstName))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(a => a.LastName)
                    //.HasColumnName(nameof(Order.ShippingAddress.LastName))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.AddressLine)
                    //.HasColumnName(nameof(Order.ShippingAddress.AddressLine))
                    .HasMaxLength(180)
                    .IsRequired();

                addressBuilder.Property(a => a.Country)
                    //.HasColumnName(nameof(Order.ShippingAddress.Country))
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.State)
                    //.HasColumnName(nameof(Order.ShippingAddress.State))
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.ZipCode)
                    //.HasColumnName(nameof(Order.ShippingAddress.ZipCode))
                    .HasMaxLength(5)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.BillingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();

                addressBuilder.Property(a => a.Country)
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.State)
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });
        
        builder.ComplexProperty(
            o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardName)
                    //.HasColumnName(nameof(Order.Payment.CardName))
                    .HasMaxLength(50);

                paymentBuilder.Property(p => p.CardNumber)
                    //.HasColumnName(nameof(Order.Payment.CardNumber))
                    .HasMaxLength(24)
                    .IsRequired();

                paymentBuilder.Property(p => p.Expiration)
                    //.HasColumnName(nameof(Order.Payment.Expiration))
                    .HasMaxLength(10);

                paymentBuilder.Property(p => p.CVV)
                    //.HasColumnName(nameof(Order.Payment.CVV))
                    .HasMaxLength(3);

                paymentBuilder.Property(p => p.PaymentMethod);
                //.HasColumnName(nameof(Order.Payment.PaymentMethod));
            });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(o => o.TotalPrice);
    }
}
