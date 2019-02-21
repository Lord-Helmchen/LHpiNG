using LHpiNG.Album;
using LHpiNG.Cardmarket;
using LHpiNG.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LHpiNG.db.EFConfigs
{
    class SetExpansionMapConfiguration : IEntityTypeConfiguration<SetExpansionMap>
    {
        public void Configure(EntityTypeBuilder<SetExpansionMap> modelBuilder)
        {
            modelBuilder
                .Property(s => s.SetTLA)
                .HasMaxLength(3)
            ;
            modelBuilder
                .HasKey(m => new { m.SetTLA, m.ExpansionEnName });
            modelBuilder
                .HasOne<Set>(m => m.Set)
                .WithOne()
                .HasPrincipalKey<Set>(s => s.TLA)
                .OnDelete(DeleteBehavior.Cascade)
            ;
            modelBuilder
                .HasOne<Expansion>(m => m.Expansion)
                .WithOne()
                .HasPrincipalKey<Expansion>(e => e.EnName)
                .OnDelete(DeleteBehavior.Cascade)
            ;

        }


    }
}
