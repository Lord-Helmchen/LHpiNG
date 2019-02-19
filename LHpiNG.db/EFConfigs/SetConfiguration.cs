using LHpiNG.Album;
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
    class SetConfiguration : IEntityTypeConfiguration<Set>
    {
        public void Configure(EntityTypeBuilder<Set> modelBuilder)
        {
            modelBuilder
                .Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(s => s.Name)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(s => s.TLA)
                .HasMaxLength(3)
                .IsRequired()
                .ValueGeneratedNever()
            ;

            modelBuilder
                .HasKey(s => s.Id)
            ;
            modelBuilder
                .HasAlternateKey(s => s.TLA)
            ;
            modelBuilder
                .HasIndex(s => s.Name)
                .IsUnique()
            ;
            modelBuilder
                .HasMany<AlbumObject>(s => s.AlbumObjects)
                .WithOne(o => o.Set)
                .HasPrincipalKey(s => s.TLA)
                .OnDelete(DeleteBehavior.Cascade)
            ;
        }
    }
}
