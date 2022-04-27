using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Image).HasMaxLength(300).IsRequired(false);
            builder.Property(x => x.Tille).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Details).IsRequired().HasMaxLength(5000);
            builder.Property(x => x.Starttime).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}
