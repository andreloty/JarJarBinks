using JarJarBinks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace JarJarBinks.Infra.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(@"User")
                   .HasKey(h => h.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(d => d.Name)
                   .HasColumnName("Name")
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(d => d.Age)
                   .HasColumnName("Age")
                   .IsRequired();

            builder.Property(d => d.IsActive)
                   .HasColumnName("IsActive")
                   .HasDefaultValue(true);
        }
    }
}
