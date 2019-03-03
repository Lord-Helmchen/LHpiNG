﻿using LHpiNG.Util;
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
        public DbSet<Card> Cards { get; set; }
        //Maps
        public DbSet<SetExpansionMap> SetExpansionMaps { get; set; }
        public DbSet<CardProductMap> CardProductMaps { get; set; }

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
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new SetConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new CardProductMapConfiguration());
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
                    .Include(s => s.Cards)
                        .ThenInclude(o => o.Language)
                    .ToList();
                return sets;

            }
            catch (DbException) //should catch both SqlExcelption and SqliteException
            {
                throw;
            }
        }
        public IEnumerable<Card> LoadObjects()
        {
            try
            {
                var cards = new List<Card>();
                cards = Cards
                    .ToList();
                return cards;

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
                        foreach (Card card in set.Cards)
                        {
                            AddOrUpdateCard(card);
                        }
                        set.Cards = existing.Cards;
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
        }
        public void SaveCards(IEnumerable<Card> cards)
        {
            int i = 1;
            foreach (Card card in cards)
            {
                Console.Write($"\r {i}");
                AddOrUpdateCard(card);
                i++;
            }
            Console.WriteLine();
        }

        private void AddOrUpdateCard(Card card)
        {
            try
            {
                Card existing = Cards.Find(card.OracleName, card.Version, card.SetTLA, card.ObjectType, card.LanguageTLA);
                if (existing != null)
                {
                    existing.InjectNonNull(card);
                }
                else
                {
                    Cards.Add(card);
                }
            }
            catch (DbException)
            {
                throw;
            }
        }
        #endregion

        #region maps
        public HashSet<SetExpansionMap> LoadSetAtlas()
        {
            try
            {
                var setAtlas = new List<SetExpansionMap>();
                setAtlas = SetExpansionMaps
                    .Include(m => m.Set)
                    .Include(m => m.Expansion)
                    .ToList();
                return setAtlas.ToHashSet();
            }
            catch (DbException) //should catch both SqlExcelption and SqliteException
            {
                throw;
            }

        }
        public void SaveSetAtlas(ICollection<SetExpansionMap> setAtlas)
        {
            foreach (SetExpansionMap map in setAtlas)
            {
                SetExpansionMap existing = SetExpansionMaps.Find(map.SetTLA, map.ExpansionUid);
                if (existing != null)
                {

                }
                else
                {
                    SetExpansionMaps.Add(map);
                }
            }
        }
        public HashSet<CardProductMap> LoadCardAtlas()
        {
            try
            {
                var cardAtlas = new List<CardProductMap>();
                cardAtlas = CardProductMaps
                    .Include(m => m.Card)
                    .Include(m => m.Product)
                    .ToList();
                return cardAtlas.ToHashSet();
            }
            catch (DbException) //should catch both SqlExcelption and SqliteException
            {
                throw;
            }

        }
        public void SaveCardAtlas(ICollection<CardProductMap> cardAtlas)
        {
            foreach (CardProductMap map in cardAtlas)
            {
                CardProductMap existing = CardProductMaps.Find(map.CardUid, map.ProductUid);
                if (existing != null)
                {

                }
                else
                {
                    CardProductMaps.Add(map);
                }
            }
        }
        #endregion
    }
}
