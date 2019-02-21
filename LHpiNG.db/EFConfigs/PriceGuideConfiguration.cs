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
    class PriceGuideConfiguration : IEntityTypeConfiguration<PriceGuide>
    {
        public void Configure(EntityTypeBuilder<PriceGuide> modelBuilder)
        {
            //from PriceGuideEntity
            modelBuilder
                .Property(g => g.FetchDate)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property("ProductName")
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property("ExpansionName")
                .IsRequired()
                .ValueGeneratedNever()
            ;

            modelBuilder
                .HasKey("ProductName", "ExpansionName", "FetchDate")
            ;
            //additional for PriceGuide
            modelBuilder
                .Property(o => o.Uid)
                .IsRequired()
                .ValueGeneratedOnAdd()
            ;
            modelBuilder
                .HasAlternateKey(o => o.Uid)
            ;
            modelBuilder
                .HasOne<Product>(g => g.Product)
                .WithMany(p => p.PriceGuides)
                .HasForeignKey("ProductName", "ExpansionName")//shadow property (db columns but no field in model)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade)
            ;
        }
    }
}
