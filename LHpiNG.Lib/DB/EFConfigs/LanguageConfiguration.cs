using LHpiNG.Album;
using LHpiNG.Cardmarket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.DB.EFConfigs
{
    class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> modelBuilder)
        {
            modelBuilder
                .Property(l => l.Id)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(l => l.Name)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder
                .Property(l => l.TLA)
                .IsRequired()
                .ValueGeneratedNever()
                .HasMaxLength(3)
            ;
            modelBuilder
                .Property(l => l.M15Abbr)
                .HasMaxLength(2)
            ;
            modelBuilder
                .HasAlternateKey(l => l.TLA)
                ;
        }
    }
}
