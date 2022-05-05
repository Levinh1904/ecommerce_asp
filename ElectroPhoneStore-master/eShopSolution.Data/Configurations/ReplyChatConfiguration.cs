using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configurations
{
    public class ReplyChatConfiguration : IEntityTypeConfiguration<ReplyChat>
    {
        public void Configure(EntityTypeBuilder<ReplyChat> builder)
        {
            builder.ToTable("ReplyChats");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Message).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Reply).IsRequired().HasMaxLength(5000);
        }
    }
}

