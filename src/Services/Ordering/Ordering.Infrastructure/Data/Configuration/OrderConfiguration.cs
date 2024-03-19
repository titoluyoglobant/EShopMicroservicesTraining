using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configuration;

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
            .HasForeignKey(o => o.CustomerId);

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(
            o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(name => name.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(address => address.FirstName)
                    //.HasColumnName(nameof(Order.ShippingAddress.FirstName))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(address => address.LastName)
                    //.HasColumnName(nameof(Order.ShippingAddress.LastName))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(address => address.EmailAddress)
                    .HasMaxLength(50);

                addressBuilder.Property(address => address.AddressLine)
                    //.HasColumnName(nameof(Order.ShippingAddress.AddressLine))
                    .HasMaxLength(180)
                    .IsRequired();

                addressBuilder.Property(address => address.Country)
                    //.HasColumnName(nameof(Order.ShippingAddress.Country))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(address => address.State)
                    //.HasColumnName(nameof(Order.ShippingAddress.State))
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(address => address.ZipCode)
                    //.HasColumnName(nameof(Order.ShippingAddress.ZipCode))
                    .HasMaxLength(5)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(payment => payment.CardName)
                    //.HasColumnName(nameof(Order.Payment.CardName))
                    .HasMaxLength(50)
                    .IsRequired();

                paymentBuilder.Property(payment => payment.CardNumber)
                    //.HasColumnName(nameof(Order.Payment.CardNumber))
                    .HasMaxLength(24)
                    .IsRequired();

                paymentBuilder.Property(payment => payment.Expiration)
                    //.HasColumnName(nameof(Order.Payment.Expiration))
                    .HasMaxLength(10)
                    .IsRequired();

                paymentBuilder.Property(payment => payment.CVV)
                    //.HasColumnName(nameof(Order.Payment.CVV))
                    .HasMaxLength(3)
                    .IsRequired();

                paymentBuilder.Property(payment => payment.PaymentMethod);
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
