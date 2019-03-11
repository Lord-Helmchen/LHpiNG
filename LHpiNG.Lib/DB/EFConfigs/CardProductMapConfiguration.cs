using LHpiNG.Album;
using LHpiNG.Cardmarket;
using LHpiNG.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LHpiNG.DB.EFConfigs
{
    class CardProductMapConfiguration : IEntityTypeConfiguration<CardProductMap>
    {
        public void Configure(EntityTypeBuilder<CardProductMap> modelBuilder)
        {
            modelBuilder
                .Property(m => m.CardUid)
                .HasColumnType("binary(32)")
            ;
            modelBuilder
                .Property(m => m.ProductUid)
                .HasColumnType("binary(32)")
            ;
            modelBuilder
                .HasKey(m => new { m.CardUid, m.ProductUid });
            modelBuilder
                .HasOne<Card>(m => m.Card)
                .WithOne()
                .HasPrincipalKey<Card>(o => o.Uid)
                .OnDelete(DeleteBehavior.Cascade)
            ;
            modelBuilder
                .HasOne<Product>(m => m.Product)
                .WithOne()
                .HasPrincipalKey<Product>(p => p.Uid)
                .OnDelete(DeleteBehavior.Cascade)
            ;

        }


    }
}
