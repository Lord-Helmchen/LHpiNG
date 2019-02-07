using LHpiNg.Util;
using LHpiNG.Cardmarket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.db
{
    public abstract class EFContext : DbContext, ICardmarketData, IAlbumData
    {
        public DbSet<Expansion> Expansions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PriceGuide> PriceGuides { get; set; }

        protected EFContext() : base()
        {// just pass along to DbContext()
            this.Database.Migrate();//https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/#apply-migrations-at-runtime
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("LHpi");

            // column names
            modelBuilder.Entity<ProductEntity>()
                .Property(p => p.EnName)
                .HasColumnName("Name")
                ;
            modelBuilder.Entity<ExpansionEntity>()
                .Property(e => e.EnName)
                .HasColumnName("Name")
                .ValueGeneratedNever()
                ;
            // primary keys
            modelBuilder.Entity<ProductEntity>()
                .HasKey(p => new { p.EnName, p.ExpansionName });
            modelBuilder.Entity<ExpansionEntity>()
                .HasKey(e => e.EnName)
                ;

            //foreign keys
            modelBuilder.Entity<ProductEntity>()
                .HasOne<ExpansionEntity>(p => p.Expansion)
                .WithMany()
                .HasForeignKey(p => new { p.EnName, p.ExpansionName })
                ;
            //modelBuilder.Entity<ProductEntity>()
            //    .HasOne<PriceGuideEntity>(p => p.PriceGuide)
            //    .WithOne()
            //    .HasForeignKey<PriceGuideEntity>(g => g.Uid)
            //    ;
            modelBuilder.Entity<Product>()
                .HasMany<PriceGuide>(p => p.PriceGuides)
                .WithOne(g => g.Product)
                .HasForeignKey(p => p.Uid)
                ;

            modelBuilder.Entity<Expansion>()
                .HasMany<Product>(e => e.Products)
                .WithOne(p => p.Expansion as Expansion)
                .HasForeignKey(e => e.EnName)
                ;

            modelBuilder.Entity<PriceGuide>()
                .HasOne<PriceGuide>(g => g.PreviousPriceGuide)
                .WithOne()
                .HasForeignKey<PriceGuide>(g => g.Uid)
                ;
            modelBuilder.Entity<PriceGuide>()
                .HasOne<Product>(g => g.Product)
                .WithMany(p => p.PriceGuides)
                .HasForeignKey(g => g.Uid)
                ;
            modelBuilder.Entity<PriceGuideEntity>()
                .HasOne<ProductEntity>()
                .WithOne(x => x.PriceGuide)
                .HasForeignKey<PriceGuideEntity>(g => g.Uid)
                ;

        }

        // ILHpiDatabase methods
        #region Cardmarket
        /// <summary>
        /// Load all Data from db into memory
        /// </summary>
        /// <returns></returns>
        public ExpansionList LoadExpansionList()
        {
            try
            {
                var expansionList = new ExpansionList
                {
                    Expansions = Expansions.ToList()
                };
                foreach (Expansion expansion in expansionList)
                {
                    expansion.Products = LoadProducts(expansion);
                }

                return expansionList;
            }
            catch (DbException) //should catch both SqlExcelption and SqliteException
            {
                throw;
            }
        }

        public void SaveExpansionList(ExpansionList expansionList)
        {
            try
            {
                foreach (Expansion expansion in expansionList)
                {
                    AddOrUpdateExpansion(expansion);
                }
                SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }

        public Expansion LoadExpansion(Expansion expansion)
        {
            try
            {
                expansion = Expansions.Find(expansion.EnName);
                expansion.Products = LoadProducts(expansion);
                return expansion;
            }
            catch (DbException)
            {
                throw;
            }
        }

        public void AddOrUpdateExpansion(Expansion expansion)
        {
            try
            {
                Expansion existing = Expansions.SingleOrDefault(x => x.EnName == expansion.EnName);
                if (existing != null)
                {
                    existing.InjectNonNull(expansion);
                }
                else
                {
                    Expansions.Add(expansion);
                }
                SaveProducts(expansion);
                SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }
        public Product LoadProduct(Product product)
        {
            return Products.Find(new { product.EnName, product.ExpansionName });
        }
        public IEnumerable<Product> LoadProducts(Expansion expansion)
        {
            return Products.Where(x => x.ExpansionName == expansion.EnName);
        }

        public void AddOrUpdateProduct(Product product)
        {
            try
            {
                Product existing = Products.SingleOrDefault(x => x.EnName == product.EnName && x.ExpansionName == product.ExpansionName);
                if (existing != null)
                {
                    existing.InjectNonNull(product);
                }
                else
                {
                    Products.Add(product);
                }
                SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }

        public void SaveProducts(Expansion expansion)
        {
            if (expansion.Products != null)
            {
                foreach (Product product in expansion.Products)
                {
                    AddOrUpdateProduct(product);
                }
            }
        }
        #endregion

        #region Album

        #endregion
    }
}
