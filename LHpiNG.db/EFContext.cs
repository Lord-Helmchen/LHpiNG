using LHpiNG.Util;
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
        public DbSet<State> State { get; set; }

        protected EFContext() : base()
        {// just pass along to DbContext()
            //this.Database.Migrate();//https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/#apply-migrations-at-runtime
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("LHpi");

            modelBuilder.Entity<ExpansionEntity>()
                .Property(e => e.EnName)
                .HasColumnName("Name")
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder.Entity<ExpansionEntity>()
                 .HasKey(e => e.EnName)
            ;
            modelBuilder.Entity<Expansion>()
                .HasMany<Product>(e => e.Products)
                .WithOne()
                .HasForeignKey("ExpansionName")
                .HasConstraintName("FK_Products_Expansions_ExpansionName")
                .OnDelete(DeleteBehavior.Cascade)
            ;

            modelBuilder.Entity<ProductEntity>()
                .Property(p => p.EnName)
                .HasColumnName("Name")
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder.Entity<ProductEntity>()
                .Property(p => p.ExpansionName)
                .IsRequired()
                .ValueGeneratedNever()
            ;
            modelBuilder.Entity<ProductEntity>()
                .HasKey(p => new { p.EnName, p.ExpansionName })
            ;
            modelBuilder.Entity<ProductEntity>()
                .HasOne<ExpansionEntity>(p => p.Expansion)
                .WithMany()
                .HasForeignKey("ExpansionName")
                .HasConstraintName("FK_Products_Expansions_ExpansionName")
                .OnDelete(DeleteBehavior.Cascade)
            ;
            modelBuilder.Entity<Product>()
                .HasMany<PriceGuide>(p => p.PriceGuides)
                .WithOne(g => g.Product)
                .HasForeignKey("ProductName", "ExpansionName")
            ;

            modelBuilder.Entity<PriceGuideEntity>()
                .Property(g => g.Uid)
                .ValueGeneratedOnAdd()
            ;
            modelBuilder.Entity<PriceGuideEntity>()
               .HasKey(g => g.Uid)
            ;
            modelBuilder.Entity<PriceGuide>()
                .HasOne<Product>(g => g.Product)
                .WithMany(p => p.PriceGuides)
                .HasForeignKey("ProductName", "ExpansionName")//shadow property (db columns but no field in model)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade)
            ;
            modelBuilder.Entity<PriceGuide>()
                .HasOne<PriceGuide>(g => g.PreviousPriceGuide)
                .WithOne()//no navigational property on other end
                .HasForeignKey<PriceGuide>("PreviousPriceGuideUid")//shadow property (db columns but no field in model)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull)
            ;

            modelBuilder.Entity<State>().HasData(new { Id = 1, ExpansionListFetchDate = DateTime.MinValue });

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
                    Expansions = Expansions
                        .Include(e => e.Products)
                            .ThenInclude(p => p.PriceGuides)
                        .Include(e => e.Products)
                        .ToList()
                };
                expansionList.FetchedOn = State.LastOrDefault()?.ExpansionListFetchDate ?? DateTime.MinValue;
                foreach (Expansion expansion in expansionList)
                {
                    foreach (Product product in expansion.Products)
                    {
                        if (product.PriceGuides.Count > 0)
                        {
                            product.PriceGuide = product.PriceGuides
                                .Aggregate((minItem, nextItem) => minItem.FetchDate.Date < nextItem.FetchDate.Date ? minItem : nextItem);
                        }
                    }
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
                State.LastOrDefault().ExpansionListFetchDate = expansionList.FetchedOn;
                SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }

        [Obsolete]
        public Expansion LoadExpansion(Expansion expansion)
        {
            try
            {
                expansion = Expansions.Find(expansion.EnName);
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
                SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }

        [Obsolete]
        public Product LoadProduct(Product product)
        {
            return Products.Find(new { product.EnName, product.ExpansionName });
        }

        [Obsolete]
        public IEnumerable<Product> LoadProducts(Expansion expansion)
        {
            return Products.Where(x => x.ExpansionName == expansion.EnName);
        }

        [Obsolete]
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

        [Obsolete]
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
