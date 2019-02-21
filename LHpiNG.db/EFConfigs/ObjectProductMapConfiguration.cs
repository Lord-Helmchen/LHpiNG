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
                .HasOne<AlbumObject>(m => m.AlbumObject)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade)
            ;
            modelBuilder
                .HasOne<Product>(m => m.Product)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade)
            ;
            

        }


    }
}
