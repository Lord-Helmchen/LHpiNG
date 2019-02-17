using LHpiNG.Util;
using LHpiNG.Cardmarket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.db.EFConfigs;

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

            modelBuilder.ApplyConfiguration(new ExpansionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ExpansionConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new PriceGuideEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PriceGuideConfiguration());

            modelBuilder.Entity<State>().HasData(new { Id = 1, ExpansionListFetchDate = DateTime.MinValue });

            base.OnModelCreating(modelBuilder);
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
                        .ToList()
                };
                expansionList.FetchedOn = State.LastOrDefault()?.ExpansionListFetchDate ?? DateTime.MinValue;

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
