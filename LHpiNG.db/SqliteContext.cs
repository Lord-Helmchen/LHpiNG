using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.Cardmarket;
using SQLite.CodeFirst;

namespace LHpiNG.db
{
    //TODO compare with https://github.com/msallin/SQLiteCodeFirst/tree/master/SQLite.CodeFirst.Console

    public class SQLiteContext : DbContext, ILHpiDatabase
    {
        private string DBPath { get; set; }
        private SQLiteConnectionStringBuilder connectionStringBuilder{get; set;}


        public SQLiteContext() : base()
        {
            DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                FailIfMissing = true,
                ForeignKeys = true,
                DataSource = DBPath
    };
        }
        public SQLiteContext(String connectionString) : base(connectionString)
        {
            DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                FailIfMissing = true,
                ForeignKeys = true,
                DataSource = DBPath
            };
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<SQLiteContext>(modelBuilder);
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges <SQLiteContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public DbSet<Expansion> Expansions { get; set; }
        public DbSet<Product> Producs { get; set; }
        public DbSet<PriceGuide> PriceGuides { get; set; }

        // ILHpiDatabase methods
        #region Cardmarket
        public ExpansionList LoadExpansionList()
        {
            throw new NotImplementedException();
        }
        public void SaveExpansionList(ExpansionList expansionList)
        {
            throw new NotImplementedException();
        }
        public Expansion LoadExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public void SaveExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public Product LoadProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public void SaveProduct(Product product)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Album

        #endregion
    }
}
