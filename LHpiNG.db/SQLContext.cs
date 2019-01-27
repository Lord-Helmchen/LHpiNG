using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.db
{
    public class SQLContext : DbContext, ILHpiDatabase
    {
        public SQLContext() : base()
        {
            Database.SetInitializer<SQLContext>(new DropCreateDatabaseIfModelChanges<SQLContext>());
        }

        public SQLContext(String connectionString) : base(connectionString)
        {
            Database.SetInitializer<SQLContext>(new CreateDatabaseIfNotExists<SQLContext>());
        }

        public DbSet<Expansion> Expansions { get; set; }
        public DbSet<Product> Producs { get; set; }
        public DbSet<PriceGuide> PriceGuides { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            //Configure default schema
            builder.HasDefaultSchema("LHpi");

            builder.Entity<ExpansionEntity>()
                .HasIndex(x => x.Abbreviation)
                //.IsUnique().HasFilter("Abbreviation IS NOT NULL") // sadly only in EF Core
                .HasName("IX_tla");
        }

        // ILHpiDatabase methods
        #region Cardmarket
        public ExpansionList LoadExpansionList()
        {
#pragma warning disable IDE0017 // Simplify object initialization
            ExpansionList expansionList = new ExpansionList();
#pragma warning restore IDE0017 // Simplify object initialization
            expansionList.Expansions = Expansions.ToList();
            return expansionList;
        }
        public void SaveExpansionList(ExpansionList expansionList)
        {
            try
            {
                Expansions.AddRange(expansionList.Expansions);
                SaveChanges();
            }
            catch (SqlException)
            {
                throw;
            }
        }
        public Expansion LoadExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public void SaveExpansion(Expansion expansion)
        {
            try
            {
                Expansions.Add(expansion);
                SaveChanges();
            }
            catch (SqlException)
            {
                throw;
            }
        }
        public Product LoadProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public void SaveProduct(Product product)
        {
            throw new NotImplementedException();
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
