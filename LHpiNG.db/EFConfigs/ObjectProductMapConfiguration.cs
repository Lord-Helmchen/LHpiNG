using LHpiNG.Album;
using LHpiNG.Cardmarket;
using LHpiNG.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LHpiNG.db.EFConfigs
{
    class ObjectProductMapConfiguration : IEntityTypeConfiguration<ObjectProductMap>
    {
        public void Configure(EntityTypeBuilder<ObjectProductMap> modelBuilder)
        {
            modelBuilder
                .HasKey(m => new { m.AlbumObjectUid, m.ProductUid });
            modelBuilder
                .HasOne<AlbumObject>(m => m.AlbumObject)
                .WithOne()
                .HasPrincipalKey<AlbumObject>(o => o.Uid)
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
