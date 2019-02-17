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
    class ExpansionEntityConfiguration : IEntityTypeConfiguration<ExpansionEntity>
    {
        public void Configure(EntityTypeBuilder<ExpansionEntity> modelBuilder)
        {
            modelBuilder
                .Property(e => e.EnName)
                .HasColumnName("Name")
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                 .HasKey(e => e.EnName)
            ;
        }
    }
    class ExpansionConfiguration : IEntityTypeConfiguration<Expansion>
    {
        public void Configure(EntityTypeBuilder<Expansion> modelBuilder)
        {
            modelBuilder
                .HasMany<Product>(e => e.Products)
                .WithOne()
                .HasForeignKey("ExpansionName")
                .HasConstraintName("FK_Products_Expansions_ExpansionName")
                .OnDelete(DeleteBehavior.Cascade)
            ;
        }
    }
}
