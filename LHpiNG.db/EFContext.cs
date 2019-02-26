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
using LHpiNG.Maps;

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
        //Maps
        public DbSet<ObjectProductMap> ObjectProductMap { get; set; }
        public DbSet<SetExpansionMap> SetExpansionMap { get; set; }

        protected EFContext() : base()
        {// just pass along to DbContext()

            //TODO check whether we can and want to automatically migrate (we probably do to reduce end-user work)
            //update: we definitely don't want it in the constructor, as this messes with removing migrations
            //https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/#apply-migrations-at-runtime
            //this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("LHpi");

            modelBuilder.ApplyConfiguration(new ExpansionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ExpansionConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new PriceGuideConfiguration());
            modelBuilder.ApplyConfiguration(new AlbumObjectConfiguration());
            modelBuilder.ApplyConfiguration(new SetConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new ObjectProductMapConfiguration());
            modelBuilder.ApplyConfiguration(new SetExpansionMapConfiguration());

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
                //SaveChanges();
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
                Expansion existing = Expansions.Find(expansion.EnName);
                if (existing != null)
                {
                    foreach (Product product in expansion.Products)
                    {
                        AddOrUpdateProduct(product);
                    }
                    expansion.Products = existing.Products;
                    existing.InjectNonNull(expansion);
                }
                else
                {
                    Expansions.Add(expansion);
                }
                //SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }
        public void AddOrUpdateProduct(Product product)
        {
            try
            {
                Product existing = Products.Find(product.EnName, product.ExpansionName);
                if (existing != null)
                {
                    foreach (PriceGuide priceGuide in product.PriceGuides)
                    {
                        AddOrUpdatePriceGuide(priceGuide);
                    }
                    product.PriceGuides = existing.PriceGuides;
                    existing.InjectNonNull(product);
                }
                else
                {
                    Products.Add(product);
                }
                //Console.WriteLine($"saving {product.EnName} in {product.ExpansionName}");
                //SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }
        public void AddOrUpdatePriceGuide(PriceGuide priceGuide)
        {
            try
            {
                PriceGuide existing = PriceGuides.SingleOrDefault(g => g.Product.EnName == priceGuide.Product.EnName
                                                                    && g.Product.ExpansionName == priceGuide.Product.ExpansionName
                                                                    && g.FetchDate == priceGuide.FetchDate);
                if (existing != null)
                {
                    priceGuide.Uid = existing.Uid;
                    existing.InjectNonNull(priceGuide);
                }
                else
                {
                    PriceGuides.Add(priceGuide);
                }
                //SaveChanges();
            }
            catch (DbException)
            {
                throw;
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
                        .ThenInclude(o => o.Language)
                    .ToList();
                return sets;

            }
            catch (DbException) //should catch both SqlExcelption and SqliteException
            {
                throw;
            }
        }
        public IEnumerable<AlbumObject> LoadObjects()
        {
            try
            {
                var albumObjects = new List<AlbumObject>();
                albumObjects = AlbumObjects
                    .ToList();
                return albumObjects;

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
                    Language existing = Languages.Find(language.Id);
                    if (existing != null)
                    {
                        existing.InjectNonNull(language);
                    }
                    else
                    {
                        Languages.Add(language);
                    }
                }
                catch (DbException)
                {
                    throw;
                }
            }
            SaveChanges();
        }

        public void SaveSets(IEnumerable<Set> sets)
        {
            foreach (Set set in sets)
            {
                try
                {
                    Set existing = Sets.Find(set.Id);
                    if (existing != null)
                    {
                        foreach (AlbumObject albumObject in set.AlbumObjects)
                        {
                            AddOrUpdateAlbumObject(albumObject);
                        }
                        set.AlbumObjects = existing.AlbumObjects;
                        existing.InjectNonNull(set);
                    }
                    else
                    {
                        Sets.Add(set);
                    }
                }
                catch (DbException)
                {
                    throw;
                }
            }
            //SaveChanges();
        }
        public void SaveAlbumObjects(IEnumerable<AlbumObject> albumObjects)
        {
            int i = 1;
            foreach (AlbumObject albumObject in albumObjects)
            {
                Console.Write($"\r {i}");
                AddOrUpdateAlbumObject(albumObject);
                i++;
            }
            Console.WriteLine();
        }

        private void AddOrUpdateAlbumObject(AlbumObject albumObject)
        {
            try
            {
                AlbumObject existing = AlbumObjects.Find(albumObject.OracleName, albumObject.Version, albumObject.SetTLA, albumObject.ObjectType, albumObject.LanguageTLA);
                if (existing != null)
                {
                    existing.InjectNonNull(albumObject);
                }
                else
                {
                    AlbumObjects.Add(albumObject);
                }
                //SaveChanges();
            }
            catch (DbException)
            {
                throw;
            }
        }

        #endregion
    }
}
