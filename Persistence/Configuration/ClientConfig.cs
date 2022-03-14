using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration;

public class ClientConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clientes");
        builder.HasKey(client => client.Id);
        builder.Property(client => client.Name)
            .HasMaxLength(80)
            .IsRequired();
        builder.Property(client => client.Surname)
            .HasMaxLength(80)
            .IsRequired();
        builder.Property(client => client.BirthDate)
            .IsRequired();
        builder.Property(client => client.Phone)
            .HasMaxLength(9)
            .IsRequired();
        builder.Property(client => client.Email)
            .HasMaxLength(100);
        builder.Property(client => client.Address)
            .HasMaxLength(100);
        builder.Property(client => client.CreatedBy)
            .HasMaxLength(30);
        builder.Property(client => client.LastModifiedBy)
            .HasMaxLength(30);
    }
}