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
    class AlbumObjectConfiguration : IEntityTypeConfiguration<AlbumObject>
    {
        public void Configure(EntityTypeBuilder<AlbumObject> modelBuilder)
        {
            modelBuilder
                .Property(o => o.OracleName)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(o => o.Version)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(o => o.SetTLA)
                .HasColumnName("Set")
                .IsRequired()
                .HasMaxLength(3)
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(o => o.LanguageTLA)
                .HasColumnName("Language")
                .IsRequired()
                .HasMaxLength(3)
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(o => o.Uid)
                .HasColumnType("binary(32)")
                .IsRequired()
                .ValueGeneratedNever()
            ;

            modelBuilder
                .HasKey(o => new { o.OracleName, o.Version, o.SetTLA, o.ObjectType, o.LanguageTLA })
            ;
            modelBuilder
                .HasAlternateKey(o => o.Uid)
            ;
            modelBuilder
                .HasOne<Set>(o => o.Set)
                .WithMany(s => s.AlbumObjects)
                .HasForeignKey(o => o.SetTLA)
                .HasPrincipalKey(s => s.TLA)
                .OnDelete(DeleteBehavior.Cascade)
            ;
            modelBuilder
                .HasOne<Language>(o => o.Language)
                .WithMany()
                .HasForeignKey(o => o.LanguageTLA)
                .HasPrincipalKey(l => l.TLA)
                .OnDelete(DeleteBehavior.Restrict)
            ;
        }
    }
}
