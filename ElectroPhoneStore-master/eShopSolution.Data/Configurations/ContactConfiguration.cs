using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.SDT).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Gmail).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Image).HasMaxLength(300).IsRequired(false);
            builder.Property(x => x.Tille).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Status).IsRequired();
        }
    }
}
