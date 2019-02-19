using LHpiNG.Cardmarket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.db.EFConfigs
{
    class PriceGuideEntityConfiguration : IEntityTypeConfiguration<PriceGuideEntity>
    {
        public void Configure(EntityTypeBuilder<PriceGuideEntity> modelBuilder)
        {
            modelBuilder
                .Property(g => g.Uid)
                .ValueGeneratedOnAdd()
            ;
            modelBuilder
               .HasKey(g => g.Uid)
            ;
        }
    }
    class PriceGuideConfiguration : IEntityTypeConfiguration<PriceGuide>
    {
        public void Configure(EntityTypeBuilder<PriceGuide> modelBuilder)
        {
            modelBuilder
                .HasOne<Product>(g => g.Product)
                .WithMany(p => p.PriceGuides)
                .HasForeignKey("ProductName", "ExpansionName")//shadow property (db columns but no field in model)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade)
            ;
            modelBuilder
                .HasIndex("ProductName", "ExpansionName", "FetchDate")
                .HasFilter("FetchDate not null and ProductName not null and ExpansionName not null")
                .IsUnique()
                ;
        }
    }
}
