using LHpiNg.Util;
using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
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
        }

        protected EFContext(String connectionString) : base(connectionString)
        {// just pass along to DbContext(connectionString)
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            //Configure default schema
            builder.HasDefaultSchema("LHpi");

            //builder.Entity<ExpansionEntity>()
            //    .HasIndex(x => x.Abbreviation)
            //    //.IsUnique().HasFilter("Abbreviation IS NOT NULL") // only in EF Core :-(
            //    .HasName("IX_tla");
            //builder.Entity<ExpansionEntity>()
            //    .HasOptional(x => x.IdExpansion);
        }

        // ILHpiDatabase methods
        #region Cardmarket
        public ExpansionList LoadExpansionList()
        {
            try
            {
#pragma warning disable IDE0017 // Simplify object initialization
                var expansionList = new ExpansionList();
#pragma warning restore IDE0017 // Simplify object initialization
                expansionList.Expansions = Expansions.ToList();
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
            return Expansions.Find(expansion.EnName);
            //TODO alternatively, look for other identifying index values
        }
        public void AddOrUpdateExpansion(Expansion expansion)
        {
            try
            {
                var existing = Expansions.SingleOrDefault<Expansion>(x => x.EnName == expansion.EnName);
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
        public Product LoadProduct(Product product)
        {
            return Products.Find(new { product.EnName, product.ExpansionName });
        }
        public void AddOrUpdateProduct(Product product)
        {
            try
            {
                var existing = Products.SingleOrDefault<Product>(x => x.EnName == product.EnName && x.ExpansionName == product.ExpansionName);
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

        public IEnumerable<Product> LoadProducts(Expansion expansion)
        {
            throw new NotImplementedException();
        }

        public void SaveProducts(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Album

        #endregion
    }
}
