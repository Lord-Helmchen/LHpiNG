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
using LHpiNG.Album;

namespace LHpiNG.db
{
    public abstract class EFContext : DbContext, ICardmarketData, IAlbumData
    {
        public DbSet<State> State { get; set; }
        // Cardmarket related
        public DbSet<Expansion> Expansions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PriceGuide> PriceGuides { get; set; }
        //Album related
        public DbSet<Language> Languages { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<AlbumObject> AlbumObjects { get; set; }

        protected EFContext() : base()
        {// just pass along to DbContext()

            //TODO check whether we can and want to automatically migrate (we probably do to reduce end-user work)
            //https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/#apply-migrations-at-runtime
            this.Database.Migrate();
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
            modelBuilder.ApplyConfiguration(new AlbumObjectConfiguration());
            modelBuilder.ApplyConfiguration(new SetConfiguration());

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

        public void AddOrUpdateExpansion(Expansion expansion)
        {
            try
            {
                Expansion existing = Expansions.SingleOrDefault(e => e.EnName == expansion.EnName);
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
        public IEnumerable<Language> LoadLanguages()
        {
            try
            {
                var languages = new List<Language>();
                languages = Languages.ToList();
                return languages;
            }
            catch (DbException) //should catch both SqlExcelption and SqliteException
            {
                throw;
            }
        }
        public IEnumerable<Set> LoadSets()
        {
            try
            {
                var sets = new List<Set>();
                sets = Sets
                    .Include(s => s.AlbumObjects)
                    .ToList();
                return sets;

            }
            catch (DbException) //should catch both SqlExcelption and SqliteException
            {
                throw;
            }
        }

        public void SaveLanguages(IEnumerable<Language> languages)
        {
            foreach (Language language in languages)
            {
                try
                {
                    Language existing = Languages.SingleOrDefault(l => l.Id == language.Id);
                    if (existing != null)
                    {
                        existing.InjectNonNull(language);
                    }
                    else
                    {
                        Languages.Add(language);
                    }
                    SaveChanges();
                }
                catch (DbException)
                {
                    throw;
                }

            }
        }
        public void SaveSets(IEnumerable<Set> sets)
        {
            foreach (Set set in sets)
            {
                try
                {
                    Set existing = Sets.SingleOrDefault(l => l.Id == set.Id);
                    if (existing != null)
                    {
                        existing.InjectNonNull(set);
                    }
                    else
                    {
                        Sets.Add(set);
                    }
                    SaveChanges();
                }
                catch (DbException)
                {
                    throw;
                }

            }
        }

        #endregion
    }
}
