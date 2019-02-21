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
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            // from ProductEntity
            modelBuilder
                .Property(p => p.EnName)
                .HasColumnName("Name")
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(p => p.ExpansionName)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .HasKey(p => new { p.EnName, p.ExpansionName })
            ;
            modelBuilder
                .HasOne<ExpansionEntity>(p => p.Expansion)
                .WithMany()
                .HasForeignKey("ExpansionName")
                .HasConstraintName("FK_Products_Expansions_ExpansionName")
                .OnDelete(DeleteBehavior.Cascade)
            ;
            //additional for Product
            modelBuilder
                .Property(o => o.Uid)
                .IsRequired()
                .ValueGeneratedOnAdd()
            ;
            modelBuilder
                .HasAlternateKey(o => o.Uid)
            ;
            modelBuilder
                .HasMany<PriceGuide>(p => p.PriceGuides)
                .WithOne(g => g.Product)
                .HasForeignKey("ProductName", "ExpansionName")
            ;
        }
    }
}
